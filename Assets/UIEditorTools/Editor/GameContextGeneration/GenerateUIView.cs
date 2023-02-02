using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UIEditorTools.Editor
{
    public partial class GameContextGenerationUtility : EditorWindow
    {
        private static partial class UIViewGenerator
        {
            private static string uiViewHeader = @"
namespace {0}.Views
{{
    public partial class {1} : UIView
    {{";

            private static string uiViewInitTemplate = @"        protected override async Task Init()
        {{
{0}
        }}
";

            private static string uiViewDoneTemplate = @"        protected override async Task Done()
        {{
{0}
        }}
";

            private static string uiViewFooter = @"    }
}";

            public static List<GameObjectComponent> BuildComponentList(GameObject view, List<ICodeGenerator> generators)
            {
                List<GameObjectComponent> gocList = new List<GameObjectComponent>();
                void CheckComponentCodeName(Transform go)
                {
                    var goName = go.name;
                    // skip object names starting with underscore
                    if (!goName.StartsWith("_"))
                    {
                        var components = go.GetComponents<MonoBehaviour>();
                        foreach (var component in components)
                        {
                            foreach (var generator in generators)
                            {
                                var name = generator.ComponentControlNameCore(goName, component);
                                if (!string.IsNullOrEmpty(name))
                                {
                                    var codeName = generator.ComponentControlName(name, component);
                                    int i = 0;
                                    while (i < 1000)
                                    {
                                        if (!gocList.Any(c => c.codeName.Equals(codeName)))
                                        {
                                            var goc = new GameObjectComponent { codeName = codeName, component = component };
                                            gocList.Add(goc);
                                            return;
                                        }
                                        ++i;
                                        codeName = generator.ComponentControlName($"{name}{i}", component);
                                    }
                                    throw new Exception($"Too many objects with the same name: {name}>={i}");
                                }
                            }
                        }
                    }
                }

                void TraverseChildren(Transform go)
                {
                    CheckComponentCodeName(go);
                    for (int i = 0; i < go.childCount; ++i)
                    {
                        TraverseChildren(go.GetChild(i));
                    }
                }

                TraverseChildren(view.transform);
                return gocList;
            }

            private static List<string> internalUsingClauses = new List<string>
            {
                "System",
                "System.Threading.Tasks",
                "UnityEngine",
                "UnityEngine.UI",
                "ACFW.Views"
            };
            public static void GenerateUIView(string filename, string filenameGenerated, string uiViewName, string projectRootNamespace, List<GameObjectComponent> components, List<ICodeGenerator> generators)
            {
                if (File.Exists(filename))
                {
                    Debug.Log($"Skipping main script file {filename} - already exists");
                }
                else
                {
                    var partialUsingClauses = new GenerateUsingClauses(new List<string> { "ACFW.Views" });
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));
                    Debug.Log($"Creating empty script file {filename}");
                    using (var stream = new StreamWriter(filename))
                    {
                        stream.Write(partialUsingClauses.GetUsingClauses());
                        stream.WriteLine(string.Format(uiViewHeader, projectRootNamespace, uiViewName));
                        stream.WriteLine(uiViewFooter);
                    }
                }
                Directory.CreateDirectory(Path.GetDirectoryName(filenameGenerated));
                Debug.Log($"Generating UIView {uiViewName} to {filenameGenerated}");
                var usingClauses = new GenerateUsingClauses(internalUsingClauses);
                using (var stream = new StreamWriter(filenameGenerated))
                {
                    string body = string.Empty;
                    string init = string.Empty;
                    string done = string.Empty;
                    foreach (var component in components)
                    {
                        foreach (var generator in generators)
                        {
                            foreach (var usingClause in generator.UsingCode(component.codeName, component.component))
                            {
                                usingClauses.Add(usingClause);
                            }
                            body += generator.BodyCode(component.codeName, component.component);
                            init += generator.InitCode(component.codeName, component.component);
                            done += generator.DoneCode(component.codeName, component.component);
                        }
                    }
                    stream.Write(usingClauses.GetUsingClauses());
                    stream.WriteLine(string.Format(uiViewHeader, projectRootNamespace, uiViewName));
                    stream.WriteLine(body);
                    stream.WriteLine(uiViewInitTemplate, init);
                    stream.WriteLine(uiViewDoneTemplate, done);
                    stream.WriteLine(uiViewFooter);
                }
            }
        }
    }
}
