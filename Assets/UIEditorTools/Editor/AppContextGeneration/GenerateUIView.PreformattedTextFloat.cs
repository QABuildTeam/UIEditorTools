using UnityEditor;
using ACFW.Views;
using System.Collections.Generic;

namespace UIEditorTools.Editor
{
    public partial class AppContextGenerationUtility : EditorWindow
    {
        [CodeGeneration]
        private class GenerateUIViewOnPreformattedTextFloat : GenerateUIViewOnIValueDisplay<PreformattedTextFloat, float>
        {
            protected override List<string> UsingClauses => new List<string> { "ACFW.Views" };
        }
    }
}
