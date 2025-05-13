using System.Collections.Generic;
using UnityEngine;

namespace Core.Services.Audio
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Config/AudioConfig")]
    public sealed class AudioConfig : BaseConfig
    {
        public List<AudioData> Audio;
    }
}