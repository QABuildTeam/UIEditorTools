using SimpleAudioSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIAudioSystem
{
    public interface IAudioPlayer
    {
        void SetMusicVolume(float value);
        float GetMusicVolume();
        void PlayMusic(AudioClipDescriptor descriptor);
        void StopMusic();

        void SetSFXVolume(float value);
        float GetSFXVolume();
        void PlaySFX(AudioClipDescriptor descriptor);
        void StopSFX();
    }
}
