using ACFW.Views;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ACFW.Controllers
{
    public abstract class AppContextContainer
    {
        private readonly IEnumerable<IMVCContainer> _mvcContainers;
        private readonly string _sceneName;
        public AppContextContainer(IEnumerable<IMVCContainer> mvcContainers)
        {
            _mvcContainers = mvcContainers;
        }

        public AppContextController CreateAppContextController(ITransformManager worldManager, ITransformManager uiManager)
        {
            return new AppContextController(_mvcContainers, worldManager, uiManager, _sceneName);
        }

        public string SceneName => _sceneName;
        public string Id => GetType().Name;
    }
}
