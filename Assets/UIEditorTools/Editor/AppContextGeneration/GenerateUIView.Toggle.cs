using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

namespace UIEditorTools.Editor
{
    public partial class AppContextGenerationUtility : EditorWindow
    {
        [CodeGeneration]
        private class GenerateUIViewOnToggle : CodeGenerator<Toggle>
        {
            protected override string NamePartToRemove => "toggle";
            protected override string BodyCodeTemplate => @"        [SerializeField]
        private Toggle {0};
        public bool {1}
        {{
            get => {0}.isOn;
            set => {0}.SetIsOnWithoutNotify(value);
        }}
        public event Action<bool> {2};
        private void On{1}ToggleChanged({3})
        {{
            {2}?.Invoke(value);
        }}
        public bool Active{1}
        {{
            get => {0}.gameObject.activeInHierarchy;
            set => {0}.gameObject.SetActive(value);
        }}
        public bool Interactive{1}
        {{
            get => {0}.interactable;
            set => {0}.interactable = value;
        }}
";
            protected override string InitCodeTemplate => @"            {0}.onValueChanged.RemoveAllListeners();
            {0}.onValueChanged.AddListener(On{1}ToggleChanged);
";
            protected override string DoneCodeTemplate => @"            {0}.onValueChanged.RemoveAllListeners();
";

            protected override string ControlNameSuffix => "Toggle";

            protected override string ActionNameSuffix => "Changed";

            protected override string ActionArguments => "bool value";
            protected override List<string> UsingClauses => new List<string> { "UnityEngine.UI" };
        }
    }
}
