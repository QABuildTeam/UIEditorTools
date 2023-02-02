using UnityEngine;

namespace UIEditorTools.Editor
{
    public class GameContextGenerationSettings : ScriptableObject
    {
        public string projectRootNamespace = "ACFW.Example";

        public string uiViewFolder = "Assets/AppContextFramework/Example/Scripts/Contexts/Generated/Views";
        public string uiControllerFolder = "Assets/AppContextFramework/Example/Scripts/Contexts/Generated/ContextControllers";
        public string uiPairFolder = "Assets/AppContextFramework/Example/Scripts/Contexts/Generated/ViewControllerPairs";
        public string uiPairAssetFolder = "Assets/AppContextFramework/Example/Settings/GameContexts";
        public string gameContextFolder = "Assets/AppContextFramework/Example/Scripts/Contexts/Generated/GameContexts";
        public string gameContextAssetFolder = "Assets/AppContextFramework/Example/Settings/GameContexts";
    }
}
