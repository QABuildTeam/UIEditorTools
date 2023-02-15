using System.Threading.Tasks;
using ACFW.Controllers;
using ACFW.Example.Views;
using ACFW.Example.Environment;

namespace ACFW.Example.Controllers
{
    public partial class TestOverlayUIController : ContextController
    {
        private IEventManager EventManager => environment.Get<IEventManager>();
        private ISettingsManager SettingsManager => environment.Get<ISettingsManager>();

        private TestOverlayUIView TestOverlayView => (TestOverlayUIView)view;
        public TestOverlayUIController(TestOverlayUIView view, IServiceLocator environment) : base(view, environment)
        {
        }

        private void OnCloseOverlayAction()
        {
            EventManager.Get<TestOverlayEvents>().CloseTestOverlay?.Invoke();
        }


        private void Subscribe()
        {
            TestOverlayView.CloseOverlayAction += OnCloseOverlayAction;

        }

        private void Unsubscribe()
        {
            TestOverlayView.CloseOverlayAction -= OnCloseOverlayAction;

        }


        public override async Task Open()
        {
            TestOverlayView.Environment = environment;
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
