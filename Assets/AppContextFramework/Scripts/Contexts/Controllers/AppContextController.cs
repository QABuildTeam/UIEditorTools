using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using ACFW.Views;

namespace ACFW.Controllers
{
    public class AppContextController : IContextController
    {
        private bool isOpen = false;
        public bool IsOpen() => isOpen;

        private readonly IEnumerable<IMVCContainer> mvcContainers;
        protected ITransformManager worldManager;
        protected ITransformManager uiManager;

        public string SceneName { get; }

        public AppContextController(
            IEnumerable<IMVCContainer> mvcContainers,
            ITransformManager worldManager,
            ITransformManager uiManager,
            string sceneName)
        {
            this.mvcContainers = mvcContainers;
            this.worldManager = worldManager;
            this.uiManager = uiManager;
            SceneName = sceneName;
        }

        private object _sync = new object();
        private List<GameObjectLoader<IView>> viewLoaders = new List<GameObjectLoader<IView>>();
        private List<IContextController> controllers = new List<IContextController>();

        #region Open
        public async Task PreOpen()
        {
            if (isOpen)
            {
                return;
            }
            viewLoaders.Clear();
            controllers.Clear();

            async Task CreateVCPair(IMVCContainer mvcContainer, ITransformManager transformManager)
            {
                var loader = mvcContainer.ViewLoaderFactory.Create(transformManager.InitialTransform);
                var view = await loader.Load();
                if (view != null)
                {
                    var controller = mvcContainer.ControllerFactory.Create(view);
                    lock (_sync)
                    {
                        viewLoaders.Add(loader);
                        controllers.Add(controller);
                    }
                    await controller.PreOpen();
                    loader.LoadedObject.transform.SetParent(transformManager.WorkingTransform);
                    loader.LoadedObject.transform.localPosition = Vector3.zero;
                }
            }
            var tasks = new List<Task>();
            tasks.AddRange(mvcContainers.Where(p => p != null).Select(p => CreateVCPair(p, p.IsUI ? uiManager : worldManager)));
            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
                tasks.Clear();
            }
        }

        public virtual Task Open()
        {
            if (isOpen)
            {
                return Task.CompletedTask;
            }

            isOpen = true;

            var tasks = controllers.Select(c => c.Open()).ToList();
            return tasks.Count > 0 ? Task.WhenAll(tasks) : Task.CompletedTask;
        }

        public Task PostOpen()
        {
            if (!isOpen)
            {
                return Task.CompletedTask;
            }
            var tasks = controllers.Select(c => c.PostOpen()).ToList();
            return tasks.Count > 0 ? Task.WhenAll(tasks) : Task.CompletedTask;
        }
        #endregion

        #region Close
        public Task PreClose()
        {
            if (!isOpen)
            {
                return Task.CompletedTask;
            }
            var tasks = controllers.Select(c => c.PreClose()).ToList();
            return tasks.Count > 0 ? Task.WhenAll(tasks) : Task.CompletedTask;
        }

        public virtual async Task Close()
        {
            var tasks = controllers.Select(c => c.Close()).ToList();
            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
            }
            isOpen = false;
        }

        public async Task PostClose()
        {
            if (isOpen)
            {
                return;
            }
            var tasks = controllers.Select(c => c.PostClose()).ToList();
            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
            }
            tasks.Clear();
            foreach (var loader in viewLoaders)
            {
                loader.Dispose();
            }
            lock (_sync)
            {
                controllers.Clear();
                viewLoaders.Clear();
            }
            isOpen = false;
        }
        #endregion
    }
}
