using ACFW.Controllers;
using ACFW.Example.Environment;
using ACFW.Example.Views;

namespace ACFW.Example.Controllers
{
    public class TestOverlayUIReference : ScriptableReference<TestOverlayUIView>
    {
        public class ViewLoaderFactory : ViewLoaderFactory<TestOverlayUIView, TestOverlayUIReference>
        {
            public ViewLoaderFactory(TestOverlayUIReference reference) : base(reference)
            {
            }
        }

        public class ContextControllerFactory : IFactory<TestOverlayUIController, TestOverlayUIView>
        {
            private readonly TestOverlayEvents testOverlayEvents;

            public ContextControllerFactory(TestOverlayEvents testOverlayEvents)
            {
                this.testOverlayEvents = testOverlayEvents;
            }

            public TestOverlayUIController Create(TestOverlayUIView view)
            {
                return new TestOverlayUIController(view, testOverlayEvents);
            }
        }
    }
}
