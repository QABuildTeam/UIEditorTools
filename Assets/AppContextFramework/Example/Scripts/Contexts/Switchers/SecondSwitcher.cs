using ACFW.Environment;
using ACFW.Controllers;
using ACFW.Example.Environment;

namespace ACFW.Example.Controllers
{
    public class SecondSwitcher : AbstractAppContextSwitcher<TestAppContext>
    {
        private readonly SecondEvents secondEvents;
        public SecondSwitcher(SecondEvents secondEvents, ContextEvents contextEvents) : base(contextEvents)
        {
            this.secondEvents = secondEvents;
        }

        public override sealed void Subscribe()
        {
            secondEvents.GotoStart += SwitchContext;
        }

        public override sealed void Unsubscribe()
        {
            secondEvents.GotoStart -= SwitchContext;
        }
    }
}
