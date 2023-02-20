using UnityEngine;
using ACFW.Settings;

namespace SimpleAudioSystem.Settings
{
    [CreateAssetMenu(fileName = nameof(AudioClipSettings), menuName = "Game Settings/Audio Clip Settings")]
    public class AudioClipSettings : ScriptableSettings
    {
        public MusicTrackDescriptor[] musicTracks;
        public SFXTrackDescriptor[] sfxTracks;
    }
}
