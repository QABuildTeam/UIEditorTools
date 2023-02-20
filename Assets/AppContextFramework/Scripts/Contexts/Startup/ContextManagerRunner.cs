using UnityEngine;
using ACFW.Controllers;
using ACFW.Settings;
using ACFW.Environment;

namespace ACFW.Startup
{
    public class ContextManagerRunner : MonoBehaviour, IStartupRunner
    {
        [SerializeField]
        private ContextManager contextManager;
        public void Run(IServiceLocator environment)
        {
            var appContextList = environment.Get<ISettingsManager>().Get<AppContextList>();
            environment.Get<IEventManager>().Get<ContextEvents>().ActivateContext(appContextList.appContexts[0].Id);
        }
    }
}
