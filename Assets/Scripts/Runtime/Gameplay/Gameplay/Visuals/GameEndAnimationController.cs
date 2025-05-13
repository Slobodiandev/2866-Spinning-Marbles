using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Runtime.Gameplay.Gameplay.Visuals
{
    public class GameEndAnimationController : MonoBehaviour
    {
        private const int AnimationTime = 1;
        private const string TriggerName = "Play";
        private static readonly int Play = Animator.StringToHash(TriggerName);
    
        [SerializeField] private Animator _animator;

        public async UniTask PlayAnimation()
        {
            _animator.SetTrigger(Play);
            await UniTask.WaitForSeconds(AnimationTime);
        }
    }
}
