using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Runtime.Gameplay.SeparateSystems.UserAccount
{
    [CreateAssetMenu(fileName = "DefaultAvatarsConfig", menuName = "Config/DefaultAvatarsConfig")]
    public class DefaultAvatarsConfig : BaseConfig
    {
        [SerializeField] private List<Sprite> _avatars;
    
        public List<Sprite> Avatars => _avatars;
    }
}

