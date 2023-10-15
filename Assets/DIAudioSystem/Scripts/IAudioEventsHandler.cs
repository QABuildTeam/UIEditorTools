using ACFW;
using System;

namespace DIAudioSystem
{
    public interface IAudioEventsHandler : IDisposable
    {
        void Subscribe();
        void Unsubscribe();
    }
}
