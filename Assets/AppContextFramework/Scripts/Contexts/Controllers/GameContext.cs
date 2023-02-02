using UnityEngine;
using ACFW.Views;

namespace ACFW.Controllers
{
    public class GameContext : ScriptableObject
    {
        public ViewControllerPair[] worldObjects;
        public ViewControllerPair[] uiObjects;

        public virtual GameContextController GetContextController(ITransformManager worldManager, ITransformManager uiManager, UniversalEnvironment global)
        {
            return new GameContextController(worldObjects, worldManager, uiObjects, uiManager, sceneName, global);
        }

        public string Id => GetType().Name;
        public string sceneName = string.Empty;
    }
}
