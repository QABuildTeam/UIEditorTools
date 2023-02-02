using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using ACFW.Views;

namespace ACFW.Controllers
{
    public class GameContextController : IContextController
    {
        private bool isOpen = false;
        public bool IsOpen() => isOpen;

        protected UniversalEnvironment environment;
        protected ViewControllerPair[] worldObjects;
        protected ViewControllerPair[] uiObjects;
        protected ITransformManager worldManager;
        protected ITransformManager uiManager;

        public string SceneName { get; }

        public GameContextController(
            ViewControllerPair[] worldObjects,
            ITransformManager worldManager,
            ViewControllerPair[] uiObjects,
            ITransformManager uiManager,
            string sceneName,
            UniversalEnvironment environment)
        {
            this.worldObjects = worldObjects;
            this.worldManager = worldManager;
            this.uiObjects = uiObjects;
            this.uiManager = uiManager;
            SceneName = sceneName;
            this.environment = environment;
        }

        private object _sync = new object();
        private List<ObjectLoader<IView>> viewLoaders = new List<ObjectLoader<IView>>();
        private List<IContextController> controllers = new List<IContextController>();
        public List<IContextController> Controllers => controllers;

        #region Open
        public async Task PreOpen()
        {
            if (isOpen)
            {
                return;
            }
            viewLoaders.Clear();
            controllers.Clear();

            async Task CreateView(ViewControllerPair pair, ITransformManager transformManager)
            {
                var loader = new ObjectLoader<IView>(pair.ViewAssetReference, transformManager.InitialTransform);
                var view = await loader.Load();
                if (view != null)
                {
                    var controller = pair.GetContextController(view, environment);
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
            if (worldObjects.Length > 0)
            {
                tasks.AddRange(worldObjects.Where(p => p != null).Select(p => CreateView(p, worldManager)));
            }
            if (uiObjects.Length > 0)
            {
                tasks.AddRange(uiObjects.Where(p => p != null).Select(p => CreateView(p, uiManager)));
            }
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
            var tasks = controllers.Select(c => c.PostClose()).ToList();
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
