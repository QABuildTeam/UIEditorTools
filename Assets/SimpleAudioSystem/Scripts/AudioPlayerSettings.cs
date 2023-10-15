using UnityEngine;
using ACFW.Settings;

namespace SimpleAudioSystem
{
    [CreateAssetMenu(fileName = nameof(AudioPlayerSettings), menuName = "Game Settings/Audio Player Settings", order = 53)]
    public class AudioPlayerSettings : ScriptableSettings
    {
        public SimpleAudioPlayerSettings data;
    }
}
