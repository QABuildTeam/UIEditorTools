using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace UIEditorTools.Controllers
{
    public class GameContextSelector
    {
        private struct ContextItem
        {
            public string id;
        }

        #region ContextStack
        private class ContextStack
        {
            private List<ContextItem> stack;
            private int capacity;
            private int currentPosition;
            private int pushPosition;
            public ContextStack(int capacity)
            {
                this.capacity = capacity;
                stack = new List<ContextItem>(capacity);
                currentPosition = -1;
                pushPosition = 0;
            }

            public ContextItem Current => currentPosition >= 0 && currentPosition < stack.Count ? stack[currentPosition] : default;

            public void Push(ContextItem item)
            {
                while (pushPosition >= capacity)
                {
                    stack.RemoveAt(0);
                    --pushPosition;
                }
                if (pushPosition <= stack.Count - 1)
                {
                    stack[pushPosition] = item;
                }
                else
                {
                    stack.Add(item);
                    pushPosition = stack.Count - 1;
                }
                currentPosition = pushPosition;
                ++pushPosition;
            }

            public ContextItem Pop()
            {
                if (pushPosition > 1)
                {
                    pushPosition -= 2;
                    var result = stack[pushPosition];
                    return result;
                }
                else
                {
                    pushPosition = 0;
                    return default;
                }
            }
        }
        #endregion

        private readonly Dictionary<string, GameContextController> contextRegistry = new Dictionary<string, GameContextController>();

        private ContextStack stack;
        private ContextStack Stack => stack;
        private ContextItem CurrentContext => Stack.Current;
        public string CurrentContextId => CurrentContext.id;
        private object _sync = new object();
        private Queue<string> contextQueue;

        public GameContextSelector(int maxContentDepth = 5)
        {
            stack = new ContextStack(maxContentDepth);
            contextQueue = new Queue<string>();
        }

        public void RegisterContext(string id, GameContextController context)
        {
            if (!string.IsNullOrEmpty(id) && !contextRegistry.ContainsKey(id))
            {
                //Debug.Log($"[{GetType().Name}.{nameof(Register)}] Registering context {contextName}");
                contextRegistry.Add(id, context);
            }
        }

        private void EnqueueContext(string id)
        {
            bool startDequeueing = false;
            lock (_sync)
            {
                if (contextQueue.Count == 0)
                {
                    startDequeueing = true;
                }
                contextQueue.Enqueue(id);
            }
            if (startDequeueing)
            {
                SwitchContextFromQueue();
            }
        }

        private async void SwitchContextFromQueue()
        {
            while (true)
            {
                string nextId = string.Empty;
                bool hasNextItem = false;
                lock (_sync)
                {
                    hasNextItem = contextQueue.Count > 0;
                    if (hasNextItem)
                    {
                        nextId = contextQueue.Peek();
                    }
                }
                if (!hasNextItem)
                {
                    return;
                }
                await SwitchToContextAsync(nextId);
                lock (_sync)
                {
                    contextQueue.Dequeue();
                }
            }
        }
        private async Task LoadSceneAsync(string newSceneName)
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            if (!string.IsNullOrEmpty(newSceneName) && !newSceneName.Equals(currentSceneName))
            {
                TaskCompletionSource<bool> source = new TaskCompletionSource<bool>();
                void SetCompleted(AsyncOperation operation)
                {
                    source.TrySetResult(true);
                }
                AsyncOperation operation = SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Single);
                operation.completed += SetCompleted;
                try
                {
                    await source.Task;
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"[{GetType().Name}.{nameof(SwitchToContextAsync)}] Could not successfully load scene {newSceneName}: {ex}");
                }
                operation.completed -= SetCompleted;
            }
        }

        private async Task SwitchToContextAsync(string id)
        {
            Debug.Log($"[{GetType().Name}.{nameof(SwitchToContextAsync)}] Switching from context {CurrentContextId} to context {id}");
            if (!string.IsNullOrEmpty(id) && contextRegistry.ContainsKey(id))
            {
                if (!id.Equals(CurrentContext.id))
                {
                    var newContext = contextRegistry[id];
                    await LoadSceneAsync(newContext.SceneName);
                    var currentContext = !string.IsNullOrEmpty(CurrentContextId) ? contextRegistry[CurrentContextId] : null;
                    if (currentContext != null)
                    {
                        try
                        {
                            await currentContext.PreClose();
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError($"[{GetType().Name}.{nameof(SwitchToContextAsync)}] Could not successfully pre-close context {id}: {ex}");
                        }
                    }
                    try
                    {
                        await newContext.PreOpen();
                        await newContext.Open();
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"[{GetType().Name}.{nameof(SwitchToContextAsync)}] Could not successfully open context {id}: {ex}");
                    }
                    if (currentContext != null)
                    {
                        try
                        {
                            await currentContext.Close();
                            await currentContext.PostClose();
                        }
                        catch (System.Exception ex)
                        {
                            Debug.LogError($"[{GetType().Name}.{nameof(SwitchToContextAsync)}] Could not successfully close context {id}: {ex}");
                        }
                    }
                    try
                    {
                        await newContext.PostOpen();
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError($"[{GetType().Name}.{nameof(SwitchToContextAsync)}] Could not successfully post-open context {id}: {ex}");
                    }
                    Stack.Push(new ContextItem { id = id });
                }
            }
            else
            {
                Debug.LogError($"[{GetType().Name}.{nameof(SwitchToContextAsync)}] Context {id} is not registered");
            }
        }

        public void ActivateContext(string id)
        {
            EnqueueContext(id);
        }

        public void RestoreContext()
        {
            var previousContext = Stack.Pop();
            if (!string.IsNullOrEmpty(previousContext.id))
            {
                ActivateContext(previousContext.id);
            }
        }
    }
}
