using ACFW.Environment;
using ACFW.Controllers;
using ACFW.Example.Environment;

namespace ACFW.Example.Controllers
{
    public class TestOverlaySwitcher : AbstractAppOverlayContextSwitcher<TestOverlayAppContext>
    {
        private readonly TestOverlayEvents testOverlayEvents;
        public TestOverlaySwitcher(TestOverlayEvents testOverlayEvents, ContextEvents contextEvents) : base(contextEvents)
        {
            this.testOverlayEvents = testOverlayEvents;
        }

        public override sealed void Subscribe()
        {
            testOverlayEvents.OpenTestOverlay += OpenContext;
            testOverlayEvents.CloseTestOverlay += CloseContext;
        }

        public override sealed void Unsubscribe()
        {
            testOverlayEvents.OpenTestOverlay -= OpenContext;
            testOverlayEvents.CloseTestOverlay -= CloseContext;
        }
    }
}
