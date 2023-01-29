using UIEditorTools.Controllers;
using UIEditorTools.Environment;
using UIEditorTools.Settings;

namespace UIEditorTools.Startup
{
    public class ApplicationContext
    {
        private readonly ContextManager contextManager;
        private readonly SettingsList settings;
        private readonly GameContextList gameContextList;

        public UniversalEnvironment Environment => environment;
        private UniversalEnvironment environment;

        public static ApplicationContext Instance { get; set; }

        public ApplicationContext(ContextManager contextManager, SettingsList settings, GameContextList gameContextList)
        {
            Instance ??= this;
            this.contextManager = contextManager;
            this.settings = settings;
            this.gameContextList = gameContextList;
        }

        public void Initialize()
        {
            InitializeGlobals();
            InitializeSceneManager();
        }

        public void Run()
        {
            environment.Get<UniversalEventManager>().Get<ContextEvents>().ActivateContext(gameContextList.gameContexts[0].Id);
        }

        private void InitializeGlobals()
        {
            environment = new UniversalEnvironment();
            environment.Add<UniversalEventManager>(new UniversalEventManager());
            environment.Add<UniversalSettingsManager>(new UniversalSettingsManager(settings.settings));
        }

        private void InitializeSceneManager()
        {
            contextManager.Setup(new GameContextSelector(), gameContextList.gameContexts, environment);
        }
    }
}