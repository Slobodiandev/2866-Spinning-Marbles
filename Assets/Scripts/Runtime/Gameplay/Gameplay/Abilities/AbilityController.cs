using System;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Gameplay.Abilities
{
    public class AbilityController : MonoBehaviour
    {
        [SerializeField] private AbilityParameters _ghostAbility;
        [SerializeField] private AbilityParameters _multiColorAbility;
        [SerializeField] private AbilityParameters _queueShotAbility;

        private BallLauncher _ballLauncher;
        
        [Inject]
        private void Construct(BallLauncher ballLauncher)
        {
            _ballLauncher = ballLauncher;
        }
        
        private void Awake()
        {
            SubscribeToEvents();
            SetStartCooldowns();
        }

        private void SubscribeToEvents()
        {
            _ghostAbility.AbilityButton.Initialize(_ballLauncher.SetGhostBall, _ghostAbility.Cooldown);
            _multiColorAbility.AbilityButton.Initialize(_ballLauncher.SetMultiColorBall, _multiColorAbility.Cooldown);
            _queueShotAbility.AbilityButton.Initialize(_ballLauncher.EnableQueue, _queueShotAbility.Cooldown);
        }
        
        private void SetStartCooldowns()
        {
            _ghostAbility.AbilityButton.SetCooldown(_ghostAbility.Cooldown);
            _multiColorAbility.AbilityButton.SetCooldown(_multiColorAbility.Cooldown);
            _queueShotAbility.AbilityButton.SetCooldown(_queueShotAbility.Cooldown);
        }
        
        [Serializable]
        private class AbilityParameters
        {
            public AbilityButton AbilityButton;
            public float Cooldown;
        }
    }
}