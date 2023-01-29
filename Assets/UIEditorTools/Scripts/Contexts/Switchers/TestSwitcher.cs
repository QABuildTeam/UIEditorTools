using UIEditorTools.Environment;
using UIEditorTools.Settings;

namespace UIEditorTools.Controllers
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
            EventManager.Get<ContextEvents>().ActivateContext?.Invoke(nameof(SecondGameContext));
        }
    }
}
