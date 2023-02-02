using System.Threading.Tasks;
using ACFW.Controllers;
using ACFW.Example.Views;
using ACFW.Example.Environment;

namespace ACFW.Example.Controllers
{
    public partial class TestUIController : ContextController
    {
        private UniversalEventManager EventManager => environment.Get<UniversalEventManager>();
        private UniversalSettingsManager SettingsManager => environment.Get<UniversalSettingsManager>();

        private TestUIView TestView => (TestUIView)view;
        public TestUIController(TestUIView view, UniversalEnvironment environment) : base(view, environment)
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
