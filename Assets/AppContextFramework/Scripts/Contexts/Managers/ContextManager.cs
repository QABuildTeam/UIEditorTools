using UnityEngine;
using ACFW.Environment;
using ACFW.Views;

namespace ACFW.Controllers
{
    public class ContextManager : MonoBehaviour
    {
        private GameContextSelector selector;
        private UniversalEnvironment environment;
        private UniversalEventManager EventManager => environment?.Get<UniversalEventManager>();

        [SerializeField]
        private WorldTransformManager worldManager;
        [SerializeField]
        private UITransformManager uiManager;
        [SerializeField]
        private MasterCanvasManager masterCanvasManager;

        public void Setup(GameContextSelector selector, GameContext[] gameContexts, UniversalEnvironment environment)
        {
            this.selector = selector;
            this.environment = environment;
            if (masterCanvasManager != null)
            {
                masterCanvasManager.Init();
                environment.Add<IMasterCanvasManager>(masterCanvasManager);
            }
            foreach (var switcher in GetComponents<IContextSwitcher>())
            {
                switcher.Init(environment);
            }
            EventManager.Get<ContextEvents>().ActivateContext += OnActivateContext;
            foreach (var gameContext in gameContexts)
            {
                this.selector.RegisterContext(gameContext.Id, gameContext.GetContextController(worldManager, uiManager, environment));
            }
        }

        private void OnActivateContext(string id)
        {
            selector.ActivateContext(id);
        }
    }
}
