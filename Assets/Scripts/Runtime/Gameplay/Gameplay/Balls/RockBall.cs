using System.Collections;
using UnityEngine;

namespace Runtime.Gameplay.Gameplay.Balls
{
    public class RockBall : BallTarget
    {
        public int ActualID;
        
        private Sprite _actualSprite;

        public override void Initialize(Sprite sprite)
        {
            _actualSprite = sprite;
        }
        
        public override Sprite GetSprite() => _actualSprite;

        public int GetActualID() => ActualID;

        public void BreakRock()
        {
            _spriteRenderer.sprite = _actualSprite;
            StartCoroutine(RevealRealID());
        }

        private IEnumerator RevealRealID()
        {
            yield return null;
            BallTypeID = ActualID;
        }
    }
}