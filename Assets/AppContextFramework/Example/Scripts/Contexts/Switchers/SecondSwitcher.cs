using ACFW.Environment;
using ACFW.Controllers;
using ACFW.Example.Environment;

namespace ACFW.Example.Controllers
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
