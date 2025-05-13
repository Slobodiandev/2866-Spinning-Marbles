using UnityEngine;

namespace Runtime.Gameplay.UI
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteFullScreenResizer : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            ResizeToFitCamera();
        }

        private void ResizeToFitCamera()
        {
            Vector2 camSize = GetCameraWorldSize();
            Vector2 spriteSize = _spriteRenderer.sprite.bounds.size;

            float cameraAspect = camSize.x / camSize.y;
            float spriteAspect = spriteSize.x / spriteSize.y;

            float scaleFactor = cameraAspect < spriteAspect 
                ? camSize.y / spriteSize.y 
                : camSize.x / spriteSize.x;

            transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
            transform.position = new Vector3(0, 0, transform.position.z);
        }
    
        private Vector2 GetCameraWorldSize()
        {
            var camera = Camera.main;
            Vector2 bottomLeft = camera.ViewportToWorldPoint(new(0, 0, camera.nearClipPlane));
            Vector2 topRight = camera.ViewportToWorldPoint(new(1, 1, camera.nearClipPlane));
        
            return new Vector2(topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        }
    }
}
