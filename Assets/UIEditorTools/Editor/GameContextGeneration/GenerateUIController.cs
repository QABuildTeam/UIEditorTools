using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UIEditorTools.Editor
{
    public partial class GameContextGenerationUtility : EditorWindow
    {
        private class UIControllerGenerator
        {
            private static string uiControllerHeader = @"
namespace {0}.Controllers
{{
    public partial class {1} : ContextController
    {{
        private UniversalEventManager EventManager => environment.Get<UniversalEventManager>();
        private UniversalSettingsManager SettingsManager => environment.Get<UniversalSettingsManager>();

        private {2} {3} => ({2})view;
        public {1}({2} view, UniversalEnvironment environment) : base(view, environment)
        {{
        }}
";
            private static string onActionHandlerTemplate = @"        private void On{0}({1})
        {{
            // insert useful code here
        }}
";
            private static string onActionHandlerSubscribeTemplate = @"            {0}.{1} += On{1};
";
            private static string onActionHandlerUnsubscribeTemplate = @"            {0}.{1} -= On{1};
";
            private static string bodyTemplate = @"{0}

        private void Subscribe()
        {{
{1}
        }}

        private void Unsubscribe()
        {{
{2}
        }}
";
            private static string uiControllerFooter = @"
        public override async Task Open()
        {{
            {0}.Environment = environment;
            await base.Open();
            Subscribe();
        }}

        public override async Task Close()
        {{
            Unsubscribe();
            await base.Close();
        }}
    }}
}}";
            private static List<string> internalUsingClauses = new List<string>
            {
                "System.Threading.Tasks",
                "ACFW.Controllers"
            };
            public static void GenerateUIController(string filename, string projectRootNamespace, string uiControllerName, string uiViewName, List<GameObjectComponent> components, List<ICodeGenerator> generators)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
                Debug.Log($"Creating empty script file {filename}");
                using (var stream = new StreamWriter(filename))
                {
                    var usingClauses = new GenerateUsingClauses(internalUsingClauses);
                    usingClauses.Add(string.Format("{0}.Views", projectRootNamespace));
                    //usingClauses.Add(string.Format("{0}.Environment", projectRootNamespace));
                    stream.Write(usingClauses.GetUsingClauses());
                    string uiViewLocalName = Regex.Replace(uiViewName, "UI", string.Empty);
                    stream.WriteLine(string.Format(uiControllerHeader, projectRootNamespace, uiControllerName, uiViewName, uiViewLocalName));
                    string body = string.Empty;
                    string subscribe = string.Empty;
                    string unsubscribe = string.Empty;
                    foreach (var component in components)
                    {
                        foreach (var generator in generators)
                        {
                            var action = generator.ActionControlName(component.codeName, component.component);
                            if (!string.IsNullOrEmpty(action))
                            {
                                body += string.Format(onActionHandlerTemplate, action, generator.ActionControlArguments(component.codeName, component.component));
                                subscribe += string.Format(onActionHandlerSubscribeTemplate, uiViewLocalName, action);
                                unsubscribe += string.Format(onActionHandlerUnsubscribeTemplate, uiViewLocalName, action);
                            }
                        }
                    }
                    stream.WriteLine(string.Format(bodyTemplate, body, subscribe, unsubscribe));
                    stream.WriteLine(string.Format(uiControllerFooter, uiViewLocalName));
                }
            }
        }
    }
}
