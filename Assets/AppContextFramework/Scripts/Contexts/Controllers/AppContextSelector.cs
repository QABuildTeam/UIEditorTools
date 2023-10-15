using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using ACFW.Settings;

namespace ACFW.Controllers
{
    public class AppContextSelector
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

        #region ContextOverlays
        private class ContextOverlays
        {
            private List<ContextItem> overlays;
            public ContextOverlays(int capacity = 10)
            {
                overlays = new List<ContextItem>(capacity);
            }

            public int Contains(string id)
            {
                for (int i = 0; i < overlays.Count; ++i)
                {
                    if (overlays[i].id == id)
                    {
                        return i;
                    }
                }
                return -1;
            }

            public void Add(ContextItem item)
            {
                if (Contains(item.id) < 0)
                {
                    overlays.Add(item);
                }
            }

            public void Remove(ContextItem item)
            {
                var i = Contains(item.id);
                if (i >= 0)
                {
                    overlays.RemoveAt(i);
                }
            }
        }
        #endregion

        private readonly Dictionary<string, AppContextController> contextRegistry = new Dictionary<string, AppContextController>();

        private ContextStack stack;
        private ContextStack Stack => stack;
        private ContextItem CurrentContext => Stack.Current;
        public string CurrentContextId => CurrentContext.id;

        private ContextOverlays overlays;

        private enum ContextAction
        {
            Switch = 1,
            OpenOverlay = 2,
            CloseOverlay = 3
        }
        private struct ContextActionItem
        {
            public string id;
            public ContextAction action;
        }
        private object _sync = new object();
        private Queue<ContextActionItem> contextQueue;

        private readonly AppContextSelectorSettings settings;

        public AppContextSelector(AppContextSelectorSettings settings)
        {
            this.settings = settings;
            stack = new ContextStack(this.settings.maxStackDepth);
            overlays = new ContextOverlays();
            contextQueue = new Queue<ContextActionItem>();
        }

        public void RegisterContext(string id, AppContextController context)
        {
            if (!string.IsNullOrEmpty(id) && !contextRegistry.ContainsKey(id))
            {
                contextRegistry.Add(id, context);
            }
        }

        private void EnqueueContext(ContextAction action, string id)
        {
            bool startDequeueing = false;
            lock (_sync)
            {
                if (contextQueue.Count == 0)
                {
                    startDequeueing = true;
                }
                contextQueue.Enqueue(new ContextActionItem { action = action, id = id });
            }
            if (startDequeueing)
            {
                ProcessContextFromQueue();
            }
        }

        private bool IsActiveContext(string id)
        {
            return CurrentContextId == id;
        }

        private bool IsOverlayedContext(string id)
        {
            return overlays.Contains(id) >= 0;
        }

        private async void ProcessContextFromQueue()
        {
            ContextActionItem actionItem = new ContextActionItem { action = ContextAction.Switch, id = string.Empty };
            while (true)
            {
                bool hasNextItem = false;
                lock (_sync)
                {
                    hasNextItem = contextQueue.Count > 0;
                    if (hasNextItem)
                    {
                        actionItem = contextQueue.Peek();
                    }
                }
                if (!hasNextItem)
                {
                    return;
                }
                switch (actionItem.action)
                {
                    case ContextAction.Switch:
                        if (!IsActiveContext(actionItem.id) && !IsOverlayedContext(actionItem.id))
                        {
                            await SwitchToContextAsync(actionItem.id);
                        }
                        break;
                    case ContextAction.OpenOverlay:
                        if (!IsActiveContext(actionItem.id) && !IsOverlayedContext(actionItem.id))
                        {
                            await OpenOverlayContextAsync(actionItem.id);
                        }
                        break;
                    case ContextAction.CloseOverlay:
                        if (IsOverlayedContext(actionItem.id))
                        {
                            await CloseOverlayContextAsync(actionItem.id);
                        }
                        break;
                }
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
            if (!string.IsNullOrEmpty(id) && contextRegistry.ContainsKey(id))
            {
                Debug.Log($"[{GetType().Name}.{nameof(SwitchToContextAsync)}] Switching from context {CurrentContextId} to context {id}");
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
                            Debug.LogError($"[{GetType().Name}.{nameof(SwitchToContextAsync)}] Could not successfully pre-close context {CurrentContext.id}: {ex}");
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
                            Debug.LogError($"[{GetType().Name}.{nameof(SwitchToContextAsync)}] Could not successfully close context {CurrentContext.id}: {ex}");
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

        private async Task OpenOverlayContextAsync(string id)
        {
            if (!string.IsNullOrEmpty(id) && contextRegistry.ContainsKey(id))
            {
                Debug.Log($"[{GetType().Name}.{nameof(SwitchToContextAsync)}] Opening overlay context {id}");
                var context = contextRegistry[id];
                try
                {
                    await context.PreOpen();
                    await context.Open();
                    await context.PostOpen();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"[{GetType().Name}.{nameof(SwitchToContextAsync)}] Could not successfully open overlay context {id}: {ex}");
                }
                overlays.Add(new ContextItem { id = id });
            }
        }

        private async Task CloseOverlayContextAsync(string id)
        {
            if (!string.IsNullOrEmpty(id) && contextRegistry.ContainsKey(id))
            {
                Debug.Log($"[{GetType().Name}.{nameof(SwitchToContextAsync)}] Closing overlay context {id}");
                var context = contextRegistry[id];
                try
                {
                    await context.PreClose();
                    await context.Close();
                    await context.PostClose();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"[{GetType().Name}.{nameof(SwitchToContextAsync)}] Could not successfully close overlay context {id}: {ex}");
                }
                overlays.Remove(new ContextItem { id = id });
            }
        }

        public void ActivateContext(string id)
        {
            EnqueueContext(ContextAction.Switch, id);
        }

        public void RestoreContext()
        {
            var previousContext = Stack.Pop();
            if (!string.IsNullOrEmpty(previousContext.id))
            {
                ActivateContext(previousContext.id);
            }
        }

        public void OpenOverlayContext(string id)
        {
            EnqueueContext(ContextAction.OpenOverlay, id);
        }

        public void CloseOverlayContext(string id)
        {
            EnqueueContext(ContextAction.CloseOverlay, id);
        }
    }
}
