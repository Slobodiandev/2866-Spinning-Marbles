using System.Collections.Generic;
using Core.Services.Audio;
using DG.Tweening;
using Runtime.Gameplay.Gameplay.Balls;
using Runtime.Gameplay.Gameplay.Systems;
using Runtime.Gameplay.Services.Audio;
using UnityEngine;

namespace Runtime.Gameplay.Gameplay.Visuals
{
    public class BallsDestructionController
    {
        private readonly Vector3 ScaleTarget = Vector3.one * 1.01f;
        private const float ScaleInAnimDuration = 0.15f;
        private const float ScaleOutAnimDuration = 0.07f;
    
        private readonly IAudioService _audioService;
    
        public BallsDestructionController(BallCollisionProcessor ballCollisionProcessor, IAudioService audioService)
        {
            _audioService = audioService;
            
            ballCollisionProcessor.OnBallsMatched += DestroyBalls;
            ballCollisionProcessor.OnTriangleHit += DestroyBall;
        }

        public void DestroyBall(BallTarget ball)
        {
            PlayDestructionAnimation(ball);
        }

        public void DestroyBalls(HashSet<BallTarget> balls)
        {
            _audioService.PlaySound(ConstAudio.MatchSound);
            foreach (var ball in balls)
            {
                if (ball != null)
                    PlayDestructionAnimation(ball);
            }
        }

        private void PlayDestructionAnimation(BallTarget ball)
        {
            ball.DisableCollider();

            Transform ballTransform = ball.transform;
            GameObject ballObject = ball.gameObject;
        
            AnimatePositionPop(ballTransform, ballObject);
            AnimateScaleSequence(ballTransform, ballObject);
            AnimateFadeOut(ball, ballObject);
        }

        private void AnimatePositionPop(Transform target, GameObject linkedObject)
        {
            Vector3 originalPosition = target.position;
            Vector3 popOffset = Random.insideUnitSphere * 0.1f;

            target.DOMove(originalPosition + popOffset, ScaleInAnimDuration * 0.5f)
                .SetEase(Ease.OutSine)
                .SetLoops(2, LoopType.Yoyo)
                .SetLink(linkedObject);
        }

        private void AnimateScaleSequence(Transform target, GameObject linkedObject)
        {
            target.DOScale(ScaleTarget, ScaleInAnimDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    target.DOScale(Vector3.zero, ScaleOutAnimDuration)
                        .SetEase(Ease.InBack)
                        .OnComplete(() => Object.Destroy(linkedObject))
                        .SetLink(linkedObject);
                })
                .SetLink(linkedObject);
        }

        private void AnimateFadeOut(BallTarget ball, GameObject linkedObject)
        {
            SpriteRenderer spriteRenderer = ball.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) return;

            float fadeDuration = ScaleOutAnimDuration + ScaleInAnimDuration * 0.5f;

            spriteRenderer.DOFade(0, fadeDuration)
                .SetEase(Ease.InQuad)
                .SetLink(linkedObject);
        }
    }
}
