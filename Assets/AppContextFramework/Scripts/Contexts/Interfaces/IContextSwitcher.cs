using System;

namespace ACFW
{
    public interface IContextSwitcher : IDisposable
    {
        void Subscribe();
        void Unsubscribe();
    }
}
