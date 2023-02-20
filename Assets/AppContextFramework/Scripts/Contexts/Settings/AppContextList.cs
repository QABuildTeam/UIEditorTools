using UnityEngine;
using ACFW.Controllers;

namespace ACFW.Settings
{
    [CreateAssetMenu(fileName = nameof(AppContextList), menuName = "Game Settings/App Context List", order = 101)]
    public class AppContextList : ScriptableSettings
    {
        [Tooltip("The first entry is the start context")]
        public AppContext[] appContexts;
    }
}
