using ACFW.Controllers;
using ACFW.Example.Environment;
using ACFW.Example.Views;
using ACFW.Views;
using System;

namespace ACFW.Example.Controllers
{
    public class SecondUIReference : ScriptableReference<SecondUIView>
    {
        public class ViewLoaderFactory : ViewLoaderFactory<SecondUIView, SecondUIReference>
        {
            public ViewLoaderFactory(SecondUIReference reference) : base(reference)
            {
            }
        }

        public class ContextControllerFactory : IFactory<SecondUIController, SecondUIView>
        {
            private readonly SecondEvents secondEvents;
            private readonly TestOverlayEvents testOverlayEvents;
            public ContextControllerFactory(SecondEvents secondEvents, TestOverlayEvents testOverlayEvents)
            {
                this.secondEvents = secondEvents;
                this.testOverlayEvents = testOverlayEvents;
            }

            public SecondUIController Create(SecondUIView view)
            {
                return new SecondUIController(view, secondEvents, testOverlayEvents);
            }
        }
    }
}
