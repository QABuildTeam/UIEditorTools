using UnityEngine;
using ACFW.Environment;
using ACFW.Views;
using System.Collections.Generic;
using System;

namespace ACFW.Controllers
{
    public class ContextManager : IDisposable
    {
        private readonly AppContextSelector selector;
        private readonly ContextEvents contextEvents;
        private readonly WorldTransformManager worldManager;
        private readonly UITransformManager uiManager;
        private readonly IMasterCanvasManager masterCanvasManager;
        private readonly IEnumerable<IContextSwitcher> contextSwitchers;

        public ContextManager(
            AppContextSelector selector,
            IEnumerable<AppContextContainer> appContexts,
            ContextEvents contextEvents,
            WorldTransformManager worldTransformManager,
            UITransformManager uiTransformManager,
            IEnumerable<IContextSwitcher> switchers,
            IMasterCanvasManager masterCanvasManager)
        {
            this.selector = selector;
            this.masterCanvasManager = masterCanvasManager;

            foreach (var appContext in appContexts)
            {
                this.selector.RegisterContext(appContext.Id, appContext.CreateAppContextController(worldManager, uiManager));
            }
            contextSwitchers = switchers;

            contextEvents.ActivateContext += OnActivateContext;
            contextEvents.RestoreContext += OnRestoreContext;
            contextEvents.OpenOverlayContext += OnOpenOverlayContext;
            contextEvents.CloseOverlayContext += OnCloseOverlayContext;
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

        public void Dispose()
        {
            contextEvents.ActivateContext -= OnActivateContext;
            contextEvents.RestoreContext -= OnRestoreContext;
            contextEvents.OpenOverlayContext -= OnOpenOverlayContext;
            contextEvents.CloseOverlayContext -= OnCloseOverlayContext;
        }
    }
}
