using System.Threading.Tasks;
using ACFW.Controllers;
using ACFW.Example.Views;
using ACFW.Example.Environment;
using System;

namespace ACFW.Example.Controllers
{
    public partial class SecondUIController : ContextController
    {
        private IEventManager EventManager => environment.Get<IEventManager>();
        private ISettingsManager SettingsManager => environment.Get<ISettingsManager>();

        private SecondUIView SecondView => (SecondUIView)view;
        public SecondUIController(SecondUIView view, IServiceLocator environment) : base(view, environment)
        {
        }

        private void OnGotoStartContextAction()
        {
            EventManager.Get<SecondEvents>().GotoStart?.Invoke();
        }

        private void OnOpenOverlayAction()
        {
            EventManager.Get<TestOverlayEvents>().OpenTestOverlay?.Invoke();
        }


        private void Subscribe()
        {
            SecondView.GotoStartContextAction += OnGotoStartContextAction;
            SecondView.OpenOverlayAction += OnOpenOverlayAction;

        }

        private void Unsubscribe()
        {
            SecondView.GotoStartContextAction -= OnGotoStartContextAction;
            SecondView.OpenOverlayAction -= OnOpenOverlayAction;

        }


        public override async Task Open()
        {
            SecondView.Environment = environment;
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
