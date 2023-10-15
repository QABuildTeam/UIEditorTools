using ACFW.Environment;

namespace ACFW.Controllers
{
    public abstract class AbstractAppContextSwitcher<TAppContext> : IContextSwitcher where TAppContext : AppContext
    {
        protected readonly ContextEvents contextEvents;
        public AbstractAppContextSwitcher(ContextEvents contextEvents)
        {
            this.contextEvents = contextEvents;
        }
        public void Dispose()
        {
            Unsubscribe();
        }
        public abstract void Subscribe();
        public abstract void Unsubscribe();
        protected virtual void SwitchContext()
        {
            contextEvents.ActivateContext?.Invoke(typeof(TAppContext).Name);
        }
    }
}
