using UnityEngine;
using ACFW.Controllers;
using ACFW.Example.Views;

namespace ACFW.Example.Controllers
{
    public class SecondWorldPair : ViewControllerPair<SecondWorldController, SecondWorldView>
    {
        protected override SecondWorldController GetContextController(SecondWorldView view, IServiceLocator environment)
        {
            return new SecondWorldController(view, environment);
        }
    }
}
