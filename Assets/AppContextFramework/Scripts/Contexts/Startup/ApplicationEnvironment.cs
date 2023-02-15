using ACFW.Controllers;
using ACFW.Environment;
using ACFW.Settings;

namespace ACFW.Startup
{
    public class ApplicationEnvironment : IApplicationEnvironment
    {
        private readonly ContextManager contextManager;
        private readonly SettingsList settings;
        private readonly AppContextList appContextList;

        private IServiceLocator environment;
        public IServiceLocator Environment => environment;

        public ApplicationEnvironment(ContextManager contextManager, SettingsList settings, AppContextList appContextList)
        {
            this.contextManager = contextManager;
            this.settings = settings;
            this.appContextList = appContextList;
        }

        public virtual void Initialize()
        {
            InitializeGlobals();
            InitializeSceneManager();
        }

        public virtual void Run()
        {
            environment.Get<IEventManager>().Get<ContextEvents>().ActivateContext(appContextList.appContexts[0].Id);
        }

        protected virtual void InitializeGlobals()
        {
            environment = new UniversalEnvironment();
            environment.Add<IEventManager>(new UniversalEventManager());
            environment.Add<ISettingsManager>(new UniversalSettingsManager(settings.Settings));
        }

        protected virtual void InitializeSceneManager()
        {
            contextManager.Setup(new AppContextSelector(), appContextList.appContexts, environment);
        }
    }
}
