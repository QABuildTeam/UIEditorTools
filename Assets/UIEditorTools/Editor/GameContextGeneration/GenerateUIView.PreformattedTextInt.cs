using UnityEngine.UI;
using UnityEditor;
using Views.Common;
using System.Collections.Generic;

namespace UIEditorTools.Editor
{
    public partial class GameContextGenerationUtility : EditorWindow
    {
        [CodeGeneration]
        private class GenerateUIViewOnPreformattedTextInt : GenerateUIViewOnIValueDisplay<PreformattedTextInt, int>
        {
            protected override List<string> UsingClauses => new List<string> { "Views.Common" };
        }
    }
}
