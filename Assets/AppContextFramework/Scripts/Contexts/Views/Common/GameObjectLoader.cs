using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ACFW.Views
{
    public class GameObjectLoader<T> : IGameObjectLoader<T>
    {
        public GameObject LoadedObject { get; private set; } = null;
        public T Component { get; private set; } = default;

        private AssetReference reference = null;
        private Transform parentAnchor = null;
        private AsyncOperationHandle<GameObject> handle;

        public GameObjectLoader(AssetReference reference, Transform parentAnchor)
        {
            this.reference = reference;
            this.parentAnchor = parentAnchor;
        }

        public async Task<T> Load()
        {
            if (reference != null && LoadedObject == null)
            {
                handle = Addressables.InstantiateAsync(reference, parentAnchor);
                await handle.Task;
                if(handle.Status == AsyncOperationStatus.Succeeded)
                {
                    LoadedObject = handle.Result;
                    Component = LoadedObject.GetComponent<T>();
                    if (Component == null)
                    {
                        Debug.LogWarning($"[{GetType().Name}.{nameof(Load)}] Successfully loaded {reference?.SubObjectName}, but the component {typeof(T).Name} is null");
                    }
                }
                else
                {
                    LoadedObject = null;
                    Component = default;
                    Debug.LogWarning($"[{GetType().Name}.{nameof(Load)}] Could not load {reference.SubObjectName}");
                }
            }
            return Component;
        }

        public void Dispose()
        {
            (Component as IDisposable)?.Dispose();
            Component = default;
            LoadedObject = null;
            parentAnchor = null;
            reference = null;
            Addressables.ReleaseInstance(handle);
        }
    }
}
