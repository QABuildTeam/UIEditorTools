using UnityEngine;
using UIEditorTools.Environment;
using UIEditorTools.Controllers;
using UIEditorTools.Views;

namespace UIEditorTools.Settings
{
    public class SecondUIPair : ViewControllerPair<SecondUIController, SecondUIView>
    {
        protected override SecondUIController GetContextController(SecondUIView view, UniversalEnvironment environment)
        {
            return new SecondUIController(view, environment);
        }
    }
}
