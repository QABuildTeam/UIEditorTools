using UnityEngine;

namespace ACFW.Settings
{
    [CreateAssetMenu(fileName = nameof(SettingsList), menuName = "Game Settings/Settings List", order = 101)]
    public class SettingsList : ScriptableObject
    {
        public ScriptableObject[] settings;
    }
}
