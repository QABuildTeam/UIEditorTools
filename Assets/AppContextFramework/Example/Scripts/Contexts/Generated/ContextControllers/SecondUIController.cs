using System.Threading.Tasks;
using ACFW.Controllers;
using ACFW.Example.Views;
using ACFW.Example.Environment;
using System;

namespace ACFW.Example.Controllers
{
    public partial class SecondUIController : ContextController
    {
        private readonly SecondEvents secondEvents;
        private readonly TestOverlayEvents testOverlayEvents;

        private SecondUIView SecondView => (SecondUIView)view;
        public SecondUIController(SecondUIView view, SecondEvents secondEvents, TestOverlayEvents testOverlayEvents) : base(view)
        {
            this.secondEvents = secondEvents;
            this.testOverlayEvents = testOverlayEvents;
        }

        private void OnGotoStartContextAction()
        {
            secondEvents.GotoStart?.Invoke();
        }

        private void OnOpenOverlayAction()
        {
            testOverlayEvents.OpenTestOverlay?.Invoke();
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
