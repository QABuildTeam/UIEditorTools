using System.Threading.Tasks;
using ACFW.Controllers;
using ACFW.Example.Views;
using ACFW.Example.Environment;
using ACFW.Views;

namespace ACFW.Example.Controllers
{
    public partial class TestUIController : ContextController
    {
        private readonly TestEvents testEvents;

        private TestUIView TestView => (TestUIView)view;
        public TestUIController(IView view, TestEvents testEvents) : base(view)
        {
            this.testEvents = testEvents;
        }

        private void OnSwitchToNextContextAction()
        {
            testEvents.SwitchToNextContext?.Invoke();
        }

        private void Subscribe()
        {
            TestView.SwitchToNextContextAction += OnSwitchToNextContextAction;
        }

        private void Unsubscribe()
        {
            TestView.SwitchToNextContextAction -= OnSwitchToNextContextAction;
        }

        public override async Task Open()
        {
            await base.Open();
            Subscribe();
        }

        public override async Task Close()
        {
            Unsubscribe();
            await base.Close();
        }
    }
}
