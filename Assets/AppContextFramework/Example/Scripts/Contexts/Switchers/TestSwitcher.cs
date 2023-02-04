using ACFW.Environment;
using ACFW.Controllers;
using ACFW.Example.Environment;

namespace ACFW.Example.Controllers
{
    public class TestSwitcher : AbstractContextSwitcher
    {
        protected override void Subscribe()
        {
            EventManager.Get<TestEvents>().SwitchToNextContext += OnSwitchToNextContext;
        }

        protected override void Unsubscribe()
        {
            EventManager.Get<TestEvents>().SwitchToNextContext -= OnSwitchToNextContext;
        }

        private void OnSwitchToNextContext()
        {
            EventManager.Get<ContextEvents>().ActivateContext?.Invoke(nameof(SecondAppContext));
        }
    }
}
