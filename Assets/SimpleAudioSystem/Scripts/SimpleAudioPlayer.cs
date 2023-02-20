using UnityEngine;
using DG.Tweening;

namespace SimpleAudioSystem
{
    public class SimpleAudioPlayer : MonoBehaviour
    {
        private class ActualAudioPlayer
        {
            public AudioSource player;
        }

        private abstract class AbstractBasicAudioPlayer
        {
            /// <summary>
            /// Create a cluster of audio players
            /// </summary>
            /// <param name="playersCount">Num</param>
            /// <returns></returns>
            private ActualAudioPlayer[] CreatPlayers(GameObject root)
            {
                var players = new ActualAudioPlayer[playersCount];
                for (int i = 0; i < playersCount; ++i)
                {
                    players[i] = new ActualAudioPlayer
                    {
                        player = root.AddComponent<AudioSource>()
                    };
                    players[i].player.playOnAwake = false;
                    players[i].player.volume = 0;
                }
                return players;
            }

            protected string volumeStorageKey;
            protected float defaultVolume;
            public ActualAudioPlayer[] Players { get; private set; }
            protected int playersCount;
            protected int currentPlayer;

            protected float Volume
            {
                get => PlayerPrefs.GetFloat(volumeStorageKey, defaultVolume);
                set
                {
                    PlayerPrefs.SetFloat(volumeStorageKey, Mathf.Clamp(value, 0, 1));
                    PlayerPrefs.Save();
                }
            }

            public void SetVolume(float value)
            {
                Volume = value;
                for (int i = 0; i < playersCount; ++i)
                {
                    if (Players[i].player.isPlaying)
                    {
                        Players[i].player.volume = Volume;
                    }
                }
            }

            public float GetVolume() => Volume;

            public AbstractBasicAudioPlayer(
                GameObject root,
                int playersCount,
                string volumeStorageKey,
                float defaultVolume)
            {
                this.playersCount = playersCount;
                Players = CreatPlayers(root);
                this.volumeStorageKey = volumeStorageKey;
                this.defaultVolume = defaultVolume;
            }

            public abstract void Play(AudioClipDescriptor descriptor);
            public abstract void Stop();
        }

        private class BasicMusicPlayer : AbstractBasicAudioPlayer
        {
            protected float volumeFadeDuration;
            public BasicMusicPlayer(GameObject root, int playersCount, string volumeStorageKey, float defaultVolume, float volumeFadeDuration) :
                base(root, playersCount, volumeStorageKey, defaultVolume)
            {
                this.volumeFadeDuration = volumeFadeDuration;
            }

            private void SwitchMusicTrack(AudioClip newClip, float volumeFactor)
            {
                if (newClip != Players[currentPlayer].player.clip)
                {
                    if (Players[currentPlayer].player.clip != null)
                    {
                        int fadeOutPlayerIndex = currentPlayer;
                        DOTween.To(
                            () => Players[fadeOutPlayerIndex].player.volume,
                            (value) => Players[fadeOutPlayerIndex].player.volume = value,
                            0,
                            volumeFadeDuration
                            ).OnComplete(
                            () => Players[fadeOutPlayerIndex].player.Stop());
                        currentPlayer = (currentPlayer + 1) % playersCount;
                    }
                    Players[currentPlayer].player.volume = 0;
                    Players[currentPlayer].player.clip = newClip;
                    if (newClip != null)
                    {
                        Players[currentPlayer].player.loop = true;
                        Players[currentPlayer].player.Play();
                        int fadeInPlayerIndex = currentPlayer;
                        float musicVolume = Volume * volumeFactor;
                        DOTween.To(
                            () => Players[fadeInPlayerIndex].player.volume,
                            (value) => Players[fadeInPlayerIndex].player.volume = value,
                            musicVolume,
                            volumeFadeDuration);
                    }
                    else
                    {
                        Players[currentPlayer].player.loop = false;
                        Players[currentPlayer].player.Stop();
                    }
                }
            }

            public override void Play(AudioClipDescriptor descriptor)
            {
                SwitchMusicTrack(descriptor.clip, descriptor.volumeNormalizingFactor);
            }

            public override void Stop()
            {
                SwitchMusicTrack(null, 1);
            }
        }

        private class BasicSFXPlayer : AbstractBasicAudioPlayer
        {
            public BasicSFXPlayer(GameObject root, int playersCount, string volumeStorageKey, float defaultVolume) :
                base(root, playersCount, volumeStorageKey, defaultVolume)
            { }

            public override void Play(AudioClipDescriptor descriptor)
            {
                for (int i = 0; i < playersCount; ++i)
                {
                    if (!Players[i].player.isPlaying)
                    {
                        currentPlayer = i;
                        break;
                    }
                }
                currentPlayer %= playersCount;
                Players[currentPlayer].player.Stop();
                Players[currentPlayer].player.clip = descriptor.clip;
                Players[currentPlayer].player.volume = Volume * descriptor.volumeNormalizingFactor;
                Players[currentPlayer].player.loop = descriptor.loop;
                Players[currentPlayer].player.Play();
            }

            public override void Stop()
            {
                for (int i = 0; i < playersCount; ++i)
                {
                    Players[i].player.Stop();
                }
            }
        }


        private BasicMusicPlayer musicPlayer;
        private BasicSFXPlayer sfxPlayer;
        public void Init(SimpleAudioPlayerSettings settings)
        {
            musicPlayer = new BasicMusicPlayer(gameObject, settings.musicPlayersCount, settings.currentMusicVolumeKey, settings.defaultMusicVolume, settings.volumeFadeDuration);
            sfxPlayer = new BasicSFXPlayer(gameObject, settings.sfxPlayersCount, settings.currentSFXVolumeKey, settings.defaultSFXVolume);
        }

        public void SetMusicVolume(float value) => musicPlayer.SetVolume(value);
        public float GetMusicVolume() => musicPlayer.GetVolume();
        public void PlayMusic(AudioClipDescriptor descriptor) => musicPlayer.Play(descriptor);
        public void StopMusic() => musicPlayer.Stop();

        public void SetSFXVolume(float value) => sfxPlayer.SetVolume(value);
        public float GetSFXVolume() => sfxPlayer.GetVolume();
        public void PlaySFX(AudioClipDescriptor descriptor) => sfxPlayer.Play(descriptor);
        public void StopSFX() => sfxPlayer.Stop();
    }
}
