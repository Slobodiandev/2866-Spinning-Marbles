using UnityEngine;

namespace Runtime.Gameplay.Gameplay.Balls
{
    public class BallTarget : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected CircleCollider2D _collider2D;
    
        [SerializeField] protected int BallTypeID = 0;
    
        public float Radius => _collider2D.radius / 2;
    
        public virtual Sprite GetSprite () => _spriteRenderer.sprite;

        public virtual void Initialize(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        public void Initialize(Sprite sprite, int ballTypeID)
        {
            _spriteRenderer.sprite = sprite;
            BallTypeID = ballTypeID;
        }
    
        public int GetBallTypeID() => BallTypeID;
        public void DisableCollider() => _collider2D.enabled = false;
        public SpriteRenderer GetSpriteRenderer() => _spriteRenderer;
    }
}
