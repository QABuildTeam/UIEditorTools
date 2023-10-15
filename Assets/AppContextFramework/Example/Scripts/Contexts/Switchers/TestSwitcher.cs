using ACFW.Environment;
using ACFW.Controllers;
using ACFW.Example.Environment;

namespace ACFW.Example.Controllers
{
    public class TestSwitcher : AbstractAppContextSwitcher<SecondAppContext>
    {
        private readonly TestEvents testEvents;
        public TestSwitcher(TestEvents testEvents, ContextEvents contextEvents):base(contextEvents)
        {
            this.testEvents = testEvents;
        }

        public override sealed void Subscribe()
        {
            testEvents.SwitchToNextContext += SwitchContext;
        }

        public override sealed void Unsubscribe()
        {
            testEvents.SwitchToNextContext -= SwitchContext;
        }
    }
}
