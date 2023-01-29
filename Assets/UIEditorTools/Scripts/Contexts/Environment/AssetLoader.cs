using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UIEditorTools.Environment
{
    public class AssetLoader<T> where T:class
    {
        public T Asset { get; private set; } = null;
        private AsyncOperationHandle<T> handle;
        public AssetReference Reference { get; private set; } = null;

        public AssetLoader(AssetReference reference)
        {
            Reference = reference;
        }

        public async Task<T> Load()
        {
            if (Reference != null && Asset == null)
            {
                handle = Addressables.LoadAssetAsync<T>(Reference);
                await handle.Task;
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    Asset = handle.Result;
                }
                else
                {
                    Debug.LogWarning($"[{GetType().Name}.{nameof(Load)}] Could not load {Reference.SubObjectName}");
                    Asset = null;
                }
            }
            return Asset;
        }

        public void Dispose()
        {
            Asset = null;
            Reference = null;
            if (handle.IsValid())
            {
                Addressables.ReleaseInstance(handle);
            }
        }
    }
}
