using System.Linq;
using System;
using SimpleAudioSystem;
using SimpleAudioSystem.Environment;
using SimpleAudioSystem.Settings;

namespace DIAudioSystem
{
    public class AudioManager : IDisposable
    {
        private readonly IAudioPlayer audioPlayer;
        private readonly IAudioEventsHandler audioEventHandler;
        private readonly MusicTrackDescriptor[] musicTracks;
        private readonly SFXTrackDescriptor[] sfxTracks;
        private readonly AudioEvents audioEvents;

        public AudioManager(AudioClipSettings settings, IAudioPlayer audioPlayer, IAudioEventsHandler audioEventHandler, AudioEvents audioEvents)
        {
            musicTracks = settings.musicTracks;
            sfxTracks = settings.sfxTracks;
            this.audioPlayer = audioPlayer;
            this.audioEventHandler = audioEventHandler;
            this.audioEvents = audioEvents;

            this.audioEvents.PlayMusic += OnPlayMusic;
            this.audioEvents.PlaySFX += OnPlaySFX;
            this.audioEvents.StopMusic += OnStopMusic;
            this.audioEvents.StopSFX += OnStopSFX;

            this.audioEventHandler.Subscribe();
        }

        public void Dispose()
        {
            OnStopMusic();
            OnStopSFX();

            audioEventHandler.Unsubscribe();

            audioEvents.PlayMusic -= OnPlayMusic;
            audioEvents.PlaySFX -= OnPlaySFX;
            audioEvents.StopMusic -= OnStopMusic;
            audioEvents.StopSFX -= OnStopSFX;
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
