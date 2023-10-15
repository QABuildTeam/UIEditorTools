using UnityEngine;
using ACFW.Views;

namespace ACFW.Controllers
{
    public class AppContext : ScriptableObject
    {
        public ScriptableReference[] worldObjects;
        public ScriptableReference[] uiObjects;

        public string Id => GetType().Name;
        public string sceneName = string.Empty;
    }
}
