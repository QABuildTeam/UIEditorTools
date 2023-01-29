using UnityEngine.UI;
using UnityEditor;
using Views.Common;
using System.Collections.Generic;

namespace UIEditorTools.Editor
{
    public partial class GameContextGenerationUtility : EditorWindow
    {
        [CodeGeneration]
        private class GenerateUIViewOnPreformattedTextIntInt : CodeGenerator<PreformattedTextIntInt>
        {
            protected override string NamePartToRemove => string.Empty;
            protected override string BodyCodeTemplate => @"        [SerializeField]
        private PreformattedTextIntInt {0};
        public void Setup{1}(int value1, int value2)
        {{
            {0}.Setup(value1, value2);
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
