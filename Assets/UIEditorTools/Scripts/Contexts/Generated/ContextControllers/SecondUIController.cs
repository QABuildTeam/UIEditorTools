using System;
using System.Linq;
using UIEditorTools;
using UIEditorTools.Controllers;
using UIEditorTools.Environment;
using UIEditorTools.Settings;
using UIEditorTools.Views;
using System.Threading.Tasks;

namespace UIEditorTools.Controllers
{
    public partial class SecondUIController : ContextController
    {
        private UniversalEventManager EventManager => environment.Get<UniversalEventManager>();
        private UniversalSettingsManager SettingsManager => environment.Get<UniversalSettingsManager>();

        private SecondUIView SecondView => (SecondUIView)view;
        public SecondUIController(SecondUIView view, UniversalEnvironment environment) : base(view, environment)
        {
        }

        private void OnGotoStartContextAction()
        {
            EventManager.Get<SecondEvents>().GotoStart?.Invoke();
        }


        private void Subscribe()
        {
            SecondView.GotoStartContextAction += OnGotoStartContextAction;

        }

        private void Unsubscribe()
        {
            SecondView.GotoStartContextAction -= OnGotoStartContextAction;

        }


        public override async Task Open()
        {
            SecondView.Environment = environment;
            await base.Open();
            Subscribe();
        }

        public override async Task Close()
        {
            Unsubscribe();
            await base.Close();
        }
    }
}
