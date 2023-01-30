using UnityEditor;
using Views.Common;
using System.Collections.Generic;

namespace UIEditorTools.Editor
{
    public partial class GameContextGenerationUtility : EditorWindow
    {
        [CodeGeneration]
        private class GenerateUIViewOnPreformattedTextString : GenerateUIViewOnIValueDisplay<PreformattedTextString, string>
        {
            protected override List<string> UsingClauses => new List<string> { "Views.Common" };
        }
    }
}
