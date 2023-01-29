using UIEditorTools.Environment;
using UIEditorTools.Settings;

namespace UIEditorTools.Controllers
{
    public class SecondSwitcher : AbstractContextSwitcher
    {
        protected override void Subscribe()
        {
            EventManager.Get<SecondEvents>().GotoStart += OnGotoStart;
        }

        protected override void Unsubscribe()
        {
            EventManager.Get<SecondEvents>().GotoStart -= OnGotoStart;
        }

        private void OnGotoStart()
        {
            EventManager.Get<ContextEvents>().ActivateContext?.Invoke(nameof(TestGameContext));
        }
    }
}
