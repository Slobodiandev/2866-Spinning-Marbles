using System.Collections.Generic;
using Core;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay
{
    public class GameObjectFactory
    {
        private readonly Dictionary<string, GameObject> _cachedAddressables;
        private readonly DiContainer _container;
        private readonly IAssetProvider _assetProvider;

        public GameObjectFactory(DiContainer container, IAssetProvider assetProvider)
        {
            _cachedAddressables = new Dictionary<string, GameObject>();
            _container = container;
            _assetProvider = assetProvider;
        }

        public GameObject Create(GameObject prototype)
        {
            return _container.InstantiatePrefab(prototype);
        }

        public TComponent Create<TComponent>(GameObject prototype) where TComponent : Component
        {
            return _container.InstantiatePrefabForComponent<TComponent>(prototype);
        }

        public TComponent Create<TComponent>(GameObject prototype, Transform parent) where TComponent : Component
        {
            return _container.InstantiatePrefabForComponent<TComponent>(prototype, parent);
        }

        public TComponent Create<TComponent>(TComponent prototype) where TComponent : Component
        {
            return _container.InstantiatePrefabForComponent<TComponent>(prototype);
        }
    }
}