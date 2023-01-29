using UnityEngine;
using UIEditorTools;
using UIEditorTools.Environment;
using UIEditorTools.Settings;
using UIEditorToolsTest.Controllers;
using UIEditorToolsTest.Views;

namespace UIEditorToolsTest.Settings
{
    public class SecondUIPair : ViewControllerPair<SecondUIController, SecondUIView>
    {
        protected override SecondUIController GetContextController(SecondUIView view, UniversalEnvironment environment)
        {
            return new SecondUIController(view, environment);
        }
    }
}
