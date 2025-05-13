using Runtime.Gameplay.Gameplay.Balls;
using Runtime.Gameplay.Gameplay.Interfaces;
using Runtime.Gameplay.Gameplay.SystemManagers;
using Runtime.Gameplay.SeparateSystems.LevelSelection;
using Runtime.Gameplay.Services.UserData.Data;

namespace Runtime.Gameplay.Gameplay.Visuals
{
    public class SkinSetter : IResettable
    {
        private readonly IUserInventoryService _userInventoryService;
        private readonly GameData _gameData;

        public SkinSetter(GameplaySystemsManager gameplaySystemsManager, GameData gameData,
            IUserInventoryService userInventoryService)
        {
            _userInventoryService = userInventoryService;
            _gameData = gameData;
        
            gameplaySystemsManager.RegisterSystem(this);
        }

        public void Reset()
        {
            var usedGameItems = _userInventoryService.GetUsedGameItems();

            foreach (var ball in _gameData.ActiveBalls)
            {
                if (ball is RockBall rockBall)
                    rockBall.Initialize(usedGameItems[rockBall.ActualID].ItemSprite);
                else
                    ball.Initialize(usedGameItems[ball.GetBallTypeID()].ItemSprite);
            }
        }
    }
}
