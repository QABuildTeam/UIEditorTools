using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UIEditorTools.Editor
{
    public class GameContextGenerationSettings : ScriptableObject
    {
        public string projectRootNamespace = "UIEditorTools";

        public string uiViewFolder = "Assets/UIEditorTools/Scripts/Contexts/Generated/Views";
        public string uiControllerFolder = "Assets/UIEditorTools/Scripts/Contexts/Generated/ContextControllers";
        public string uiPairFolder = "Assets/UIEditorTools/Scripts/Contexts/Generated/ViewControllerPairs";
        public string uiPairAssetFolder = "Assets/UIEditorTools/Settings/GameContexts";
        public string gameContextFolder = "Assets/UIEditorTools/Scripts/Contexts/Generated/GameContexts";
        public string gameContextAssetFolder = "Assets/UIEditorTools/Settings/GameContexts";
    }
}
