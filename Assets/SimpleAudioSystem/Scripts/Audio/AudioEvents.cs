using ACFW;

namespace SimpleAudioSystem.Environment
{
    public class AudioEvents : IEventProvider
    {
        public UEvent<int> PlayMusic;
        public UEvent<int> PlaySFX;
        public UEvent StopMusic;
        public UEvent StopSFX;
    }
}
