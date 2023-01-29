using UnityEngine;

namespace UIEditorTools.Settings
{
    [CreateAssetMenu(fileName = nameof(GameContextList), menuName = "Game Settings/Game Context List", order = 101)]
    public class GameContextList : ScriptableObject
    {
        [Tooltip("The first entry is the start context")]
        public GameContext[] gameContexts;
    }
}
