using UnityEngine;
using ACFW.Settings;

namespace SimpleAudioSystem
{
    [CreateAssetMenu(fileName = nameof(AudioSettings), menuName = "Game Settings/Audio Settings", order = 53)]
    public class AudioSettings : ScriptableSettings
    {
        public SimpleAudioPlayerSettings data;
    }
}
