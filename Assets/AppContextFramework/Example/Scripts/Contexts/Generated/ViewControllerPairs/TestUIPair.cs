using ACFW;
using ACFW.Controllers;
using ACFW.Example.Views;

namespace ACFW.Example.Controllers
{
    public class TestUIPair : ViewControllerPair<TestUIController, TestUIView>
    {
        protected override TestUIController GetContextController(TestUIView view, IServiceLocator environment)
        {
            return new TestUIController(view, environment);
        }
    }
}
