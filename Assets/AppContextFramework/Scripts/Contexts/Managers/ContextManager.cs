using UnityEngine;
using ACFW.Environment;
using ACFW.Views;

namespace ACFW.Controllers
{
    public class ContextManager : MonoBehaviour
    {
        private AppContextSelector selector;
        private IServiceLocator environment;
        private IEventManager EventManager => environment?.Get<IEventManager>();

        [SerializeField]
        private WorldTransformManager worldManager;
        [SerializeField]
        private UITransformManager uiManager;
        [SerializeField]
        private MasterCanvasManager masterCanvasManager;

        public void Setup(AppContextSelector selector, AppContext[] appContexts, IServiceLocator environment)
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
            EventManager.Get<ContextEvents>().RestoreContext += OnRestoreContext;
            EventManager.Get<ContextEvents>().OpenOverlayContext += OnOpenOverlayContext;
            EventManager.Get<ContextEvents>().CloseOverlayContext += OnCloseOverlayContext;
            foreach (var appContext in appContexts)
            {
                this.selector.RegisterContext(appContext.Id, appContext.GetAppContextController(worldManager, uiManager, environment));
            }
        }

        private void OnActivateContext(string id)
        {
            selector.ActivateContext(id);
        }

        private void OnRestoreContext()
        {
            selector.RestoreContext();
        }

        private void OnOpenOverlayContext(string id)
        {
            selector.OpenOverlayContext(id);
        }

        private void OnCloseOverlayContext(string id)
        {
            selector.CloseOverlayContext(id);
        }
    }
}
