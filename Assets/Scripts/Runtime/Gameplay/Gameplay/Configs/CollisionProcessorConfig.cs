using Core;
using UnityEngine;

namespace Runtime.Gameplay.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "CollisionProcessorConfig", menuName = "Config/CollisionProcessorConfig")]
    public class CollisionProcessorConfig : BaseConfig
    {
        public ContactFilter2D BallFilter => new ContactFilter2D
        {
            useTriggers = true, 
            useLayerMask = true, 
            layerMask = LayerMask.GetMask(ConstLayers.BallLayer),
        };
    
        public ContactFilter2D AllCollisionFilter => new ContactFilter2D
        {
            useTriggers = true, 
            useLayerMask = true, 
            layerMask = LayerMask.GetMask(ConstLayers.BallLayer, ConstLayers.TriangleLayer),
        };

        public float SearchRadiusMultiplier = 1.2f;
        public float TriangleHitSearchRadiusMultiplier = 1.2f;
        public int MinMatchCount = 3;
        public int CollisionArraySize = 8;
    }
}
