using UnityEngine;
using ACFW.Controllers;
using ACFW.Example.Views;

namespace ACFW.Example.Controllers
{
    public class SecondUIPair : ViewControllerPair<SecondUIController, SecondUIView>
    {
        protected override SecondUIController GetContextController(SecondUIView view, UniversalEnvironment environment)
        {
            return new SecondUIController(view, environment);
        }
    }
}
