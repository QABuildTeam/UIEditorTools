using System;
using UnityEngine;

namespace SimpleAudioSystem
{
    [Serializable]
    public class AudioClipDescriptor
    {
        public AudioClip clip;
        public float volumeNormalizingFactor = 1;
        public bool loop;
    }

    [Serializable]
    public abstract class ExtendedAudioDescriptor : AudioClipDescriptor
    {
        public string descriptiveName;
        [HideInInspector]
        public int numericType;
    }

    [Serializable]
    public sealed class MusicTrackDescriptor : ExtendedAudioDescriptor
    {
    }

    [Serializable]
    public sealed class SFXTrackDescriptor : ExtendedAudioDescriptor
    {
    }
}
