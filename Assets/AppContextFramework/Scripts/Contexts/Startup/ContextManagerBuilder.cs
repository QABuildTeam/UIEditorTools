using UnityEngine;
using ACFW.Controllers;
using ACFW.Settings;

namespace ACFW.Startup
{
    public class ContextManagerBuilder : MonoBehaviour, IStartupBuilder
    {
        [SerializeField]
        private ContextManager contextManager;
        public void PopulateEnvironment(IServiceLocator environment)
        {
            var appContextList = environment.Get<ISettingsManager>().Get<AppContextList>();
            contextManager.Setup(new AppContextSelector(), appContextList.appContexts, environment);
        }
    }
}
