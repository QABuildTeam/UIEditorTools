using System.Linq;
using UnityEngine;
using ACFW;
using ACFW.Startup;
using SimpleAudioSystem.Environment;
using SimpleAudioSystem.Settings;

namespace SimpleAudioSystem
{
    public class AudioManager : MonoBehaviour, IStartupBuilder
    {
        private SimpleAudioPlayer audioPlayer;

        private MusicTrackDescriptor[] musicTracks;
        private SFXTrackDescriptor[] sfxTracks;

        public void PopulateEnvironment(IServiceLocator environment)
        {
            var settingsManager = environment.Get<ISettingsManager>();
            var settings = settingsManager.Get<AudioClipSettings>();
            musicTracks = settings.musicTracks;
            sfxTracks = settings.sfxTracks;

            audioPlayer = gameObject.AddComponent<SimpleAudioPlayer>();
            audioPlayer.Init(settingsManager.Get<AudioSettings>().data);

            environment.Get<IEventManager>().Get<AudioEvents>().PlayMusic += OnPlayMusic;
            environment.Get<IEventManager>().Get<AudioEvents>().PlaySFX += OnPlaySFX;
            environment.Get<IEventManager>().Get<AudioEvents>().StopMusic += OnStopMusic;
            environment.Get<IEventManager>().Get<AudioEvents>().StopSFX += OnStopSFX;

            var audioEventHandler = GetComponentInChildren<IAudioEventHandler>();
            if (audioEventHandler != null)
            {
                audioEventHandler.Init(environment);
            }
        }

        private void OnPlayMusic(int music)
        {
            if (music == MusicTrackType.None)
            {
                audioPlayer.StopMusic();
            }
            else
            {
                AudioClipDescriptor descriptor = musicTracks.FirstOrDefault(d => d.numericType == music) ?? musicTracks.FirstOrDefault(d => d.numericType == MusicTrackType.Universal);
                if (descriptor != null)
                {
                    //Debug.LogWarning($"[{GetType().Name}.{nameof(OnPlayMusic)}] Playing track {descriptor.clip.name}");
                    audioPlayer.PlayMusic(descriptor);
                }
            }
        }

        private void OnPlaySFX(int sfx)
        {
            if (sfx != SFXTrackType.Empty)
            {
                if (sfx == SFXTrackType.None)
                {
                    audioPlayer.StopSFX();
                }
                else
                {
                    AudioClipDescriptor descriptor = sfxTracks.FirstOrDefault(d => d.numericType == sfx) ?? sfxTracks.FirstOrDefault(d => d.numericType == SFXTrackType.Universal);
                    if (descriptor != null)
                    {
                        //Debug.LogWarning($"[{GetType().Name}.{nameof(OnPlaySFX)}] Playing SFX {descriptor.clip.name}");
                        audioPlayer.PlaySFX(descriptor);
                    }
                }
            }
        }

        private void OnStopMusic()
        {
            audioPlayer.StopMusic();
        }

        private void OnStopSFX()
        {
            audioPlayer.StopSFX();
        }
    }
}
