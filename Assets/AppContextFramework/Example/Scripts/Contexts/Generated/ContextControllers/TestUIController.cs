using System.Threading.Tasks;
using ACFW.Controllers;
using ACFW.Example.Views;
using ACFW.Example.Environment;

namespace ACFW.Example.Controllers
{
    public partial class TestUIController : ContextController
    {
        private IEventManager EventManager => environment.Get<IEventManager>();
        private ISettingsManager SettingsManager => environment.Get<ISettingsManager>();

        private TestUIView TestView => (TestUIView)view;
        public TestUIController(TestUIView view, IServiceLocator environment) : base(view, environment)
        {
        }

        private void OnSwitchToNextContextAction()
        {
            EventManager.Get<TestEvents>().SwitchToNextContext?.Invoke();
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
            TestView.Environment = environment;
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
