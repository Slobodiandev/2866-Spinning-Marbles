using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new();

        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new();

        public async UniTask Initialize()
        {
            await Addressables.InitializeAsync();
        }

        public async UniTask<T> LoadAsset<T>(string address) where T : class
        {
            if (_completedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
                return completedHandle.Result as T;

            return await CacheOnComplete(Addressables.LoadAssetAsync<T>(address), cacheKey: address);
        }
        
        public UniTask<GameObject> InstantiateGameObject(string address) => Addressables.InstantiateAsync(address).ToUniTask();

        private async UniTask<T> CacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += completeHandle => { _completedCache[cacheKey] = completeHandle; };

            AddHandle(cacheKey, handle);

            return await handle.Task;
        }

        private void AddHandle(string key, AsyncOperationHandle handle)
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandles;
            }

            resourceHandles.Add(handle);
        }

        public void Dispose()
        {
            foreach (List<AsyncOperationHandle> resourceHandles in _handles.Values)
            {
                foreach (AsyncOperationHandle handle in resourceHandles)
                    Addressables.Release(handle);
            }

            _completedCache.Clear();
            _handles.Clear();
        }
    }
}