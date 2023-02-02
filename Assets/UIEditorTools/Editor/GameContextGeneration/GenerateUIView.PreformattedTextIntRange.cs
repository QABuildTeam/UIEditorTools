using UnityEditor;
using ACFW.Views;
using System.Collections.Generic;

namespace UIEditorTools.Editor
{
    public partial class GameContextGenerationUtility : EditorWindow
    {
        [CodeGeneration]
        private class GenerateUIViewOnPreformattedTextIntRange : GenerateUIViewOnIValueDisplay<PreformattedTextIntRange, IntRange>
        {
            protected override List<string> UsingClauses => new List<string> { "ACFW.Views" };
        }
    }
}
