using ACFW;
using ACFW.Controllers;
using ACFW.Example.Views;

namespace ACFW.Example.Controllers
{
    public class TestOverlayUIPair : ViewControllerPair<TestOverlayUIController, TestOverlayUIView>
    {
        protected override TestOverlayUIController GetContextController(TestOverlayUIView view, IServiceLocator environment)
        {
            return new TestOverlayUIController(view, environment);
        }
    }
}
