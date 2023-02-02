using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UIEditorTools.Editor
{
    public partial class GameContextGenerationUtility : EditorWindow
    {
        public interface ICodeGenerator
        {
            string BodyCode(string gameObjectName, MonoBehaviour component);
            string InitCode(string gameObjectName, MonoBehaviour component);
            string DoneCode(string gameObjectName, MonoBehaviour component);
            string ComponentControlNameCore(string gameObjectName, MonoBehaviour component);
            string ComponentControlName(string gameObjectName, MonoBehaviour component);
            string ActionControlName(string gameObjectName, MonoBehaviour component);
            string ActionControlArguments(string gameObjectName, MonoBehaviour component);
            List<string> UsingCode(string gameObjectName, MonoBehaviour component);
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        public class CodeGenerationAttribute : Attribute
        {
            public CodeGenerationAttribute() { }
        }

        public abstract class CodeGenerator<T> : ICodeGenerator where T:MonoBehaviour
        {
            #region Protected members to override in derived classes
            protected abstract string ControlNameSuffix { get; }
            protected abstract string NamePartToRemove { get; }
            protected abstract string BodyCodeTemplate { get; }
            protected abstract string InitCodeTemplate { get; }
            protected abstract string DoneCodeTemplate { get; }
            protected abstract string ActionNameSuffix { get; }
            protected abstract string ActionArguments { get; }
            protected abstract List<string> UsingClauses { get; }

            protected virtual bool IsComponentValid(MonoBehaviour component) => component is T;
            #endregion

            #region Private methods
            private string GetNameCore(string name)
            {
                var distilledName = Regex.Replace(name, "[^A-Za-z0-9]", "_");
                var core = !string.IsNullOrEmpty(NamePartToRemove) ? Regex.Replace(distilledName, NamePartToRemove, string.Empty, RegexOptions.IgnoreCase) : distilledName;
                return core;
            }

            private string ControlName(string name)
            {
                var core = GetNameCore(name);
                var controlName = core[0].ToString().ToLowerInvariant() + core.Substring(1);
                return controlName;
            }

            private string ActionName(string name)
            {
                var core = GetNameCore(name);
                var actionName = core[0].ToString().ToUpperInvariant() + core.Substring(1);
                return actionName;
            }

            private string GetCodePart(string format, string name)
            {
                var name0 = ControlName(name) + ControlNameSuffix;
                var name1 = ActionName(name);
                var name2 = ActionName(name) + ActionNameSuffix;
                var name3 = ActionArguments;
                return string.Format(format, name0, name1, name2, name3);
            }
            #endregion

            #region Public interface methods

            public string BodyCode(string gameObjectName, MonoBehaviour component)
            {
                return IsComponentValid(component) ? GetCodePart(BodyCodeTemplate, gameObjectName) : null;
            }

            public string DoneCode(string gameObjectName, MonoBehaviour component)
            {
                return IsComponentValid(component) ? GetCodePart(DoneCodeTemplate, gameObjectName) : null;
            }

            public string InitCode(string gameObjectName, MonoBehaviour component)
            {
                return IsComponentValid(component) ? GetCodePart(InitCodeTemplate, gameObjectName) : null;
            }

            public string ComponentControlNameCore(string gameObjectName, MonoBehaviour component)
            {
                return IsComponentValid(component) ? ControlName(gameObjectName) : null;
            }

            public string ComponentControlName(string gameObjectName, MonoBehaviour component)
            {
                var core = ComponentControlNameCore(gameObjectName, component);
                return !string.IsNullOrEmpty(core) ? core + ControlNameSuffix : null;
            }

            public string ActionControlName(string gameObjectName, MonoBehaviour component)
            {
                if (!string.IsNullOrEmpty(ActionNameSuffix))
                {
                    if (IsComponentValid(component))
                    {
                        var name = ActionName(gameObjectName);
                        if (!string.IsNullOrEmpty(name))
                        {
                            return name + ActionNameSuffix;
                        }
                    }
                }
                return null;
            }

            public string ActionControlArguments(string gameObjectName, MonoBehaviour component)
            {
                if (IsComponentValid(component))
                {
                    var name = ActionName(gameObjectName);
                    if (!string.IsNullOrEmpty(name))
                    {
                        return ActionArguments;
                    }
                }
                return null;
            }

            public List<string> UsingCode(string gameObjectName, MonoBehaviour component)
            {
                return UsingClauses;
            }
            #endregion
        }
    }
}
