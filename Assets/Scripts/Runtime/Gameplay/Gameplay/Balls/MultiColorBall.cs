namespace Runtime.Gameplay.Gameplay.Balls
{
    public class MultiColorBall : LaunchedBall 
    {
        protected override void ProcessNearestBall(BallTarget nearestBall)
        {
            _spriteRenderer.sprite = nearestBall.GetSprite();

            if (nearestBall is RockBall rockBall)
            {
                rockBall.BreakRock();
                BallTypeID = rockBall.GetActualID();
            }
            else
                BallTypeID = nearestBall.GetBallTypeID();
        }
    }
}
