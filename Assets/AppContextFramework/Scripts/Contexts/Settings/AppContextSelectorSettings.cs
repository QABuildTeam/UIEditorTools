using UnityEngine;

namespace ACFW.Settings
{
    [CreateAssetMenu(fileName = nameof(AppContextSelectorSettings), menuName = "Game Settings/App Context Selector Settings", order = 101)]
    public class AppContextSelectorSettings : ScriptableSettings
    {
        public int maxStackDepth;
    }
}
