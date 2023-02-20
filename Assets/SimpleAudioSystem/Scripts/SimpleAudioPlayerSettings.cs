using System;

namespace SimpleAudioSystem
{
    [Serializable]
    public class SimpleAudioPlayerSettings
    {
        public string currentMusicVolumeKey = "CurrentMusicVolume";
        public int musicPlayersCount = 2;
        public float defaultMusicVolume = 0.5f;
        public float volumeFadeDuration = 0.5f;

        public string currentSFXVolumeKey = "CurrentSFXVolume";
        public int sfxPlayersCount = 10;
        public float defaultSFXVolume = 0.5f;
    }
}
