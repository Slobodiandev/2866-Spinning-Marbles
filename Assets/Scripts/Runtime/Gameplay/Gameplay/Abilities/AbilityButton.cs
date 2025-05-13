using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Gameplay.Gameplay.Abilities
{
    public class AbilityButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _cooldownImage;

        public void Initialize(Action clickCallback, float cooldown)
        {
            _button.onClick.AddListener(() =>
            {
                clickCallback?.Invoke();
                SetCooldown(cooldown);
            });
        }

        public void SetCooldown(float cooldown)
        {
            _button.interactable = false;
            StartCoroutine(PlayCooldown(cooldown));
        }

        private IEnumerator PlayCooldown(float cooldown)
        {
            float cdLeft = cooldown;

            while (cdLeft > 0)
            {
                cdLeft -= Time.deltaTime;
                _cooldownImage.fillAmount = cdLeft / cooldown;
                yield return null;
            }
            
            _button.interactable = true;
        }
    }
}