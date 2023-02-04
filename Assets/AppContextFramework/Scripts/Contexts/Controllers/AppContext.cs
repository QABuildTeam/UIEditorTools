using UnityEngine;
using ACFW.Views;

namespace ACFW.Controllers
{
    public class AppContext : ScriptableObject
    {
        public ViewControllerPair[] worldObjects;
        public ViewControllerPair[] uiObjects;

        public virtual AppContextController GetAppContextController(ITransformManager worldManager, ITransformManager uiManager, UniversalEnvironment global)
        {
            return new AppContextController(worldObjects, worldManager, uiObjects, uiManager, sceneName, global);
        }

        public string Id => GetType().Name;
        public string sceneName = string.Empty;
    }
}
