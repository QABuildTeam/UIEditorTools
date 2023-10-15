using ACFW.Environment;

namespace ACFW.Controllers
{
    public abstract class AbstractAppOverlayContextSwitcher<TAppContext> : IContextSwitcher where TAppContext : AppContext
    {
        protected readonly ContextEvents contextEvents;
        public AbstractAppOverlayContextSwitcher(ContextEvents contextEvents)
        {
            this.contextEvents = contextEvents;
        }
        public void Dispose()
        {
            Unsubscribe();
        }
        public abstract void Subscribe();
        public abstract void Unsubscribe();
        protected virtual void OpenContext()
        {
            contextEvents.OpenOverlayContext?.Invoke(typeof(TAppContext).Name);
        }
        public virtual void CloseContext()
        {
            contextEvents.CloseOverlayContext?.Invoke(typeof(TAppContext).Name);
        }
    }
}
