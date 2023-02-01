using UnityEditor;
using TMPro;
using System.Collections.Generic;

namespace UIEditorTools.Editor
{
    public partial class GameContextGenerationUtility : EditorWindow
    {
        [CodeGeneration]
        private class GenerateUIViewOnInputField : CodeGenerator<TMP_InputField>
        {
            protected override string BodyCodeTemplate => @"        [SerializeField]
        private TMP_InputField {0};
        public string {1}
        {{
            get => {0}.text;
            set => {0}.text = value;
        }}
        public event Action<string> {2};
        private void On{1}InputFieldChanged({3})
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
            {0}.onValueChanged.AddListener(On{1}InputFieldChanged);
";
            protected override string DoneCodeTemplate => @"            {0}.onValueChanged.RemoveAllListeners();
";
            protected override string NamePartToRemove => "(inputfield|input)";
            protected override string ControlNameSuffix => "InputField";
            protected override string ActionNameSuffix => "Changed";
            protected override string ActionArguments => "string value";
            protected override List<string> UsingClauses => new List<string> { "TMPro" };
        }
    }
}
