using UnityEngine;
using UIEditorTools.Environment;
using UIEditorTools.Controllers;
using UIEditorTools.Views;

namespace UIEditorTools.Settings
{
    public class TestUIPair : ViewControllerPair<TestUIController, TestUIView>
    {
        protected override TestUIController GetContextController(TestUIView view, UniversalEnvironment environment)
        {
            return new TestUIController(view, environment);
        }
    }
}
