using UnityEngine.UI;
using UnityEditor;
using Views.Common;
using System.Collections.Generic;

namespace UIEditorTools.Editor
{
    public partial class GameContextGenerationUtility : EditorWindow
    {
        [CodeGeneration]
        private class GenerateUIViewOnPreformattedTextInt : CodeGenerator<PreformattedTextInt>
        {
            protected override string NamePartToRemove => string.Empty;
            protected override string BodyCodeTemplate => @"        [SerializeField]
        private PreformattedTextInt {0};
        public int {1}
        {{
            set => {0}.Value = value;
        }}
";
            protected override string InitCodeTemplate => string.Empty;
            protected override string DoneCodeTemplate => string.Empty;
            protected override string ControlNameSuffix => string.Empty;
            protected override string ActionNameSuffix => string.Empty;
            protected override string ActionArguments => string.Empty;
            protected override List<string> UsingClauses => new List<string> { "Views.Common" };
        }
    }
}
