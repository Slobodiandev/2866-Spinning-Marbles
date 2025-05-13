using System.Collections.Generic;
using System.Linq;
using Core;
using Runtime.Gameplay.Gameplay.Balls;
using Runtime.Gameplay.Gameplay.Interfaces;
using Runtime.Gameplay.Gameplay.SystemManagers;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using Runtime.Gameplay.Services.SettingsProvider;
using Runtime.Gameplay.Services.UserData.Data;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Gameplay
{
    public class LaunchedBallsFactory : IResettable, IInitializable
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IUserInventoryService _userInventoryService;
        private readonly GameObjectFactory _gameObjectFactory;
        private readonly GameData _gameData;
    
        private GameObject _launchedBallPrefab;
        private GameObject _ghostBallPrefab;
        private GameObject _multiColorBallPrefab;

        private int _ballTypeId;
    
        public LaunchedBallsFactory(IAssetProvider assetProvider, IUserInventoryService userInventoryService,
            GameObjectFactory gameObjectFactory, GameplaySystemsManager gameplaySystemsManager, GameData gameData)
        {
            _assetProvider = assetProvider;
            _userInventoryService = userInventoryService;
            _gameObjectFactory = gameObjectFactory;
            _gameData = gameData;
        
            gameplaySystemsManager.RegisterSystem(this);
        }
    
        public void Reset()
        {
            _ballTypeId = 0;
        }

        public async void Initialize()
        {
            _launchedBallPrefab = await _assetProvider.LoadAsset<GameObject>(ConstPrefabs.LaunchedBallPrefab);
            _ghostBallPrefab = await _assetProvider.LoadAsset<GameObject>(ConstPrefabs.GhostBallPrefab);
            _multiColorBallPrefab = await _assetProvider.LoadAsset<GameObject>(ConstPrefabs.MultiColorBallPrefab);
        }

        public LaunchedBall CreateDefaultBall()
        {
            UpdateBallIndex();
            LaunchedBall ball = _gameObjectFactory.Create<LaunchedBall>(_launchedBallPrefab);
            ball.Initialize(GetBallSprite(), _ballTypeId);

            _gameData.LaunchedBalls++;
        
            return ball;
        }

        public LaunchedBall CreateBallCopy(LaunchedBall copy) => _gameObjectFactory.Create<LaunchedBall>(copy.gameObject);

        public LaunchedBall CreateMultiColorBall() => _gameObjectFactory.Create<LaunchedBall>(_multiColorBallPrefab);

        public LaunchedBall CreateGhostBall() => _gameObjectFactory.Create<LaunchedBall>(_ghostBallPrefab);

        private void UpdateBallIndex()
        {
            var ballsInGame = _gameData.ActiveBalls.ToList();
        
            HashSet<int> ballIndexes = new();

            for (int i = 0; i < ballsInGame.Count; i++)
            {
                var ball = ballsInGame[i];
                if (ball is RockBall rockBall)
                    ballIndexes.Add(rockBall.ActualID);
                else
                    ballIndexes.Add(ballsInGame[i].GetBallTypeID());
            }
        
            if (ballIndexes.Count > 0)
            {
                List<int> sortedIndexes = ballIndexes.OrderBy(x => x).ToList();
                int currentIndex = sortedIndexes.IndexOf(_ballTypeId);
                int nextIndex = (currentIndex + 1) % sortedIndexes.Count;
                _ballTypeId = sortedIndexes[nextIndex];
            }
        }

        private Sprite GetBallSprite() => _userInventoryService.GetUsedGameItems()[_ballTypeId].ItemSprite;
    }
}
