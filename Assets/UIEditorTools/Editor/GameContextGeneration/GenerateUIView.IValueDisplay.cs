using UnityEngine;
using UnityEditor;
using Views.Common;
using System.Collections.Generic;

namespace UIEditorTools.Editor
{
    public partial class GameContextGenerationUtility : EditorWindow
    {
        public abstract class GenerateUIViewOnIValueDisplay<M, T> : CodeGenerator<M> where M : MonoBehaviour, IValueDisplay<T>
        {
            protected override string NamePartToRemove => string.Empty;
            protected override string BodyCodeTemplate => @$"        [SerializeField]
        private {typeof(M).Name} {{0}};
        public {typeof(T).Name} {{1}}
        {{{{
            set => {{0}}.Value = value;
        }}}}
";
            protected override string InitCodeTemplate => string.Empty;
            protected override string DoneCodeTemplate => string.Empty;
            protected override string ControlNameSuffix => string.Empty;
            protected override string ActionNameSuffix => string.Empty;
            protected override string ActionArguments => string.Empty;
        }
    }
}
