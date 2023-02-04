using UnityEditor;
using ACFW.Views;
using System.Collections.Generic;

namespace UIEditorTools.Editor
{
    public partial class AppContextGenerationUtility : EditorWindow
    {
        [CodeGeneration]
        private class GenerateUIViewOnPreformattedTextString : GenerateUIViewOnIValueDisplay<PreformattedTextString, string>
        {
            protected override List<string> UsingClauses => new List<string> { "ACFW.Views" };
        }
    }
}
