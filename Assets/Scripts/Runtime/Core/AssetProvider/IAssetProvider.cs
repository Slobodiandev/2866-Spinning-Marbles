using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public interface IAssetProvider : IInitializer
    {
        UniTask<T> LoadAsset<T>(string address) where T : class;
        UniTask<GameObject> InstantiateGameObject(string address);
        void Dispose();
    }
}