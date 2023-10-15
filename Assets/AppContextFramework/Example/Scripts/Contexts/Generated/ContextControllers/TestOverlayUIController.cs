using System.Threading.Tasks;
using ACFW.Controllers;
using ACFW.Example.Views;
using ACFW.Example.Environment;

namespace ACFW.Example.Controllers
{
    public partial class TestOverlayUIController : ContextController
    {
        private readonly TestOverlayEvents testOverlayEvents;

        private TestOverlayUIView TestOverlayView => (TestOverlayUIView)view;
        public TestOverlayUIController(TestOverlayUIView view, TestOverlayEvents testOverlayEvents) : base(view)
        {
            this.testOverlayEvents = testOverlayEvents;
        }

        private void OnCloseOverlayAction()
        {
            testOverlayEvents.CloseTestOverlay?.Invoke();
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
