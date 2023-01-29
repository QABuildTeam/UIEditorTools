using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;

namespace UIEditorTools.Editor
{
    public partial class GameContextGenerationUtility : EditorWindow
    {
        [CodeGeneration]
        private class GenerateUIViewOnButton : CodeGenerator<Button>
        {
            protected override string NamePartToRemove => "button";
            protected override string BodyCodeTemplate => @"
        [SerializeField]
        private Button {0};
        public event Action {2};
        private void On{1}Button({3})
        {{
            {2}?.Invoke();
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
            protected override string InitCodeTemplate => @"            {0}.onClick.RemoveAllListeners();
            {0}.onClick.AddListener(On{1}Button);
";
            protected override string DoneCodeTemplate => @"            {0}.onClick.RemoveAllListeners();
";
            protected override string ControlNameSuffix => "Button";
            protected override string ActionNameSuffix => "Action";

            protected override string ActionArguments => string.Empty;

            protected override List<string> UsingClauses => new List<string> { "UnityEngine.UI" };
        }
    }
}
