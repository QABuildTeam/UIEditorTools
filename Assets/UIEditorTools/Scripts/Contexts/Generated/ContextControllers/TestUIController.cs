using System;
using System.Linq;
using UIEditorTools.Environment;
using UIEditorTools.Settings;
using UIEditorTools.Views;
using System.Threading.Tasks;

namespace UIEditorTools.Controllers
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
