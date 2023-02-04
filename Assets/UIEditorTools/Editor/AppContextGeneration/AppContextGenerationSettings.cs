using UnityEngine;

namespace UIEditorTools.Editor
{
    public class AppContextGenerationSettings : ScriptableObject
    {
        public string projectRootNamespace = "ACFW.Example";

        public string uiViewFolder = "Assets/AppContextFramework/Example/Scripts/Contexts/Generated/Views";
        public string uiControllerFolder = "Assets/AppContextFramework/Example/Scripts/Contexts/Generated/ContextControllers";
        public string vcPairFolder = "Assets/AppContextFramework/Example/Scripts/Contexts/Generated/ViewControllerPairs";
        public string vcPairAssetFolder = "Assets/AppContextFramework/Example/Settings/AppContexts";
        public string appContextFolder = "Assets/AppContextFramework/Example/Scripts/Contexts/Generated/AppContexts";
        public string appContextAssetFolder = "Assets/AppContextFramework/Example/Settings/AppContexts";
    }
}
