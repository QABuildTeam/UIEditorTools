using ACFW.Controllers;
using ACFW.Environment;
using ACFW.Settings;

namespace ACFW.Startup
{
    public class ApplicationContext : IApplicationContext
    {
        private readonly ContextManager contextManager;
        private readonly SettingsList settings;
        private readonly GameContextList gameContextList;

        private UniversalEnvironment environment;
        public UniversalEnvironment Environment => environment;

        public ApplicationContext(ContextManager contextManager, SettingsList settings, GameContextList gameContextList)
        {
            this.contextManager = contextManager;
            this.settings = settings;
            this.gameContextList = gameContextList;
        }

        public virtual void Initialize()
        {
            InitializeGlobals();
            InitializeSceneManager();
        }

        public virtual void Run()
        {
            environment.Get<UniversalEventManager>().Get<ContextEvents>().ActivateContext(gameContextList.gameContexts[0].Id);
        }

        protected virtual void InitializeGlobals()
        {
            environment = new UniversalEnvironment();
            environment.Add<UniversalEventManager>(new UniversalEventManager());
            environment.Add<UniversalSettingsManager>(new UniversalSettingsManager(settings.settings));
        }

        protected virtual void InitializeSceneManager()
        {
            contextManager.Setup(new GameContextSelector(), gameContextList.gameContexts, environment);
        }
    }
}
