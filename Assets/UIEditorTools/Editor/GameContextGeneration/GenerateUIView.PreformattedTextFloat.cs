using UnityEditor;
using Views.Common;
using System.Collections.Generic;

namespace UIEditorTools.Editor
{
    public partial class GameContextGenerationUtility : EditorWindow
    {
        [CodeGeneration]
        private class GenerateUIViewOnPreformattedTextFloat : GenerateUIViewOnIValueDisplay<PreformattedTextFloat, float>
        {
            protected override List<string> UsingClauses => new List<string> { "Views.Common" };
        }
    }
}
