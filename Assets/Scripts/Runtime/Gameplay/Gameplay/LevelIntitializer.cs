using System.Linq;
using Core;
using Runtime.Gameplay.Gameplay.Balls;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using UnityEngine;

namespace Runtime.Gameplay.Gameplay
{
    public class LevelIntitializer
    {
        private readonly IConfigProvider _configProvider;
        private readonly GameData _gameData;
        private readonly GameTriangle _gameTriangle;
        private readonly GameObjectFactory _gameObjectFactory;

        private GameObject _spawnedLevel;
        private GameObject _spawnedAdditionalContent;
    
        public LevelIntitializer(IConfigProvider configProvider, GameData gameData, GameTriangle gameTriangle,
            GameObjectFactory gameObjectFactory)
        {
            _configProvider = configProvider;
            _gameData = gameData;
            _gameTriangle = gameTriangle;
            _gameObjectFactory = gameObjectFactory;
        }

        public void SpawnLevel()
        {
            var levelConfig = _configProvider.Get<GameConfig>().LevelConfigs[_gameData.LevelID];
        
            _spawnedLevel = _gameObjectFactory.Create(levelConfig.LevelPrefab);
            _spawnedLevel.transform.SetParent(_gameTriangle.transform);
            _spawnedLevel.transform.localPosition = Vector3.zero;
            _spawnedLevel.transform.localRotation = Quaternion.identity;

            if (levelConfig.AdditonalPrefab != null)
            {
                _spawnedAdditionalContent = _gameObjectFactory.Create(levelConfig.AdditonalPrefab);
                _spawnedAdditionalContent.transform.SetParent(_gameTriangle.transform.parent);
                _spawnedAdditionalContent.transform.localRotation = Quaternion.identity;
            }
        }

        public void DestroyLevel()
        {
            Object.Destroy(_spawnedLevel);
            Object.FindObjectsOfType<BallTarget>(true).ToList().ForEach(x => Object.Destroy(x.gameObject));
        
            if(_spawnedAdditionalContent != null)
                Object.Destroy(_spawnedAdditionalContent);
        }
    }
}
