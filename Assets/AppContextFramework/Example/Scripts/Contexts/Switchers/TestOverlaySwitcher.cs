using ACFW.Environment;
using ACFW.Controllers;
using ACFW.Example.Environment;
using System;

namespace ACFW.Example.Controllers
{
    public class TestOverlaySwitcher : AbstractContextSwitcher
    {
        protected override void Subscribe()
        {
            EventManager.Get<TestOverlayEvents>().OpenTestOverlay += OnOpenTestOverlay;
            EventManager.Get<TestOverlayEvents>().CloseTestOverlay += OnCloseTestOverlay;
        }

        protected override void Unsubscribe()
        {
            EventManager.Get<TestOverlayEvents>().OpenTestOverlay -= OnOpenTestOverlay;
            EventManager.Get<TestOverlayEvents>().CloseTestOverlay -= OnCloseTestOverlay;
        }

        private void OnOpenTestOverlay()
        {
            EventManager.Get<ContextEvents>().OpenOverlayContext?.Invoke(nameof(TestOverlayAppContext));
        }

        private void OnCloseTestOverlay()
        {
            EventManager.Get<ContextEvents>().CloseOverlayContext?.Invoke(nameof(TestOverlayAppContext));
        }
    }
}
