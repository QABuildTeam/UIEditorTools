using UnityEngine;

namespace SimpleAudioSystem.Editor
{
    [CreateAssetMenu(fileName = nameof(AudioEventSettings), menuName = "Game Settings/Audio Event Settings")]
    public class AudioEventSettings : ScriptableObject
    {
        [Tooltip("Don't forget to run Tools/Generate audio event subscribers from menu")]
        [SerializeField]
        public SFXEventDescriptor[] sfxDescriptors;
        [SerializeField]
        public MusicEventDescriptor[] musicDescriptors;
    }
}
