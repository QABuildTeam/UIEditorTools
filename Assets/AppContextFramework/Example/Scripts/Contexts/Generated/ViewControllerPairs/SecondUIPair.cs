using ACFW;
using ACFW.Controllers;
using ACFW.Example.Views;

namespace ACFW.Example.Controllers
{
    public class SecondUIPair : ViewControllerPair<SecondUIController, SecondUIView>
    {
        protected override SecondUIController GetContextController(SecondUIView view, IServiceLocator environment)
        {
            return new SecondUIController(view, environment);
        }
    }
}
