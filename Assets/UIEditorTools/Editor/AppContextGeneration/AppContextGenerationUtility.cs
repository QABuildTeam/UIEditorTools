using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

namespace UIEditorTools.Editor
{
    public partial class AppContextGenerationUtility : EditorWindow
    {

        [MenuItem("Tools/UI Editor Tools/Code generation/Generate App Context", false, 0)]
        private static void ShowWIndow()
        {
            EditorWindow.GetWindow<AppContextGenerationUtility>().LoadSettings();
        }

        private string projectRootNamespace = "ACFW.Example";

        private GameObject sourceView;

        private string uiViewFolder = "Assets/AppContextFramework/Example/Scripts/Contexts/Generated/Views";
        private string uiControllerFolder = "Assets/AppContextFramework/Example/Scripts/Contexts/Generated/ContextControllers";
        private string vcPairFolder = "Assets/AppContextFramework/Example/Scripts/Contexts/Generated/ViewControllerPairs";
        private string vcPairAssetFolder = "Assets/AppContextFramework/Example/Settings/AppContexts";
        private string appContextFolder = "Assets/AppContextFramework/Example/Scripts/Contexts/Generated/AppContexts";
        private string appContextAssetFolder = "Assets/AppContextFramework/Example/Settings/AppContexts";

        private bool generateUIView = true;
        private bool generateUIController = true;
        private bool generateVCPair = true;
        private bool generateAppContext = true;

        private static string appContextGenerationSettingsAssetPath = "Assets/UIEditorTools/Settings/AppContextGenerationSettings.asset";

        private AppContextGenerationSettings CreateSettings()
        {
            var settings = ScriptableObject.CreateInstance<AppContextGenerationSettings>();
            settings.projectRootNamespace = projectRootNamespace;
            settings.uiViewFolder = uiViewFolder;
            settings.uiControllerFolder = uiControllerFolder;
            settings.vcPairFolder = vcPairFolder;
            settings.vcPairAssetFolder = vcPairAssetFolder;
            settings.appContextFolder = appContextFolder;
            settings.appContextAssetFolder = appContextAssetFolder;
            return settings;
        }
        private AppContextGenerationUtility LoadSettings()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(appContextGenerationSettingsAssetPath));
            AppContextGenerationSettings settings = AssetDatabase.LoadAssetAtPath<AppContextGenerationSettings>(appContextGenerationSettingsAssetPath);
            if (settings == null)
            {
                settings = CreateSettings();
                AssetDatabase.CreateAsset(settings, appContextGenerationSettingsAssetPath);
            }
            projectRootNamespace = settings.projectRootNamespace;
            uiViewFolder = settings.uiViewFolder;
            uiControllerFolder = settings.uiControllerFolder;
            vcPairFolder = settings.vcPairFolder;
            vcPairAssetFolder = settings.vcPairAssetFolder;
            appContextFolder = settings.appContextFolder;
            appContextAssetFolder = settings.appContextAssetFolder;
            return this;
        }

        private void SaveSettings()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(appContextGenerationSettingsAssetPath));
            var settings = CreateSettings();
            AssetDatabase.CreateAsset(settings, appContextGenerationSettingsAssetPath);
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
        }

        private void OnGUI()
        {
            GUILayout.Label("Generate an app context from a view");

            sourceView = EditorGUILayout.ObjectField("Source view prefab", sourceView, typeof(GameObject), true) as GameObject;

            projectRootNamespace= EditorGUILayout.TextField("Project root namespace", projectRootNamespace);

            generateUIView = EditorGUILayout.Toggle("Generate UI View", generateUIView);
            uiViewFolder = EditorGUILayout.TextField("Code folder", uiViewFolder);

            generateUIController = EditorGUILayout.Toggle("Generate UI Controller", generateUIController);
            uiControllerFolder = EditorGUILayout.TextField("Code folder", uiControllerFolder);

            generateVCPair = EditorGUILayout.Toggle("Generate VC Pair", generateVCPair);
            vcPairFolder = EditorGUILayout.TextField("Code folder", vcPairFolder);
            vcPairAssetFolder = EditorGUILayout.TextField("Asset folder", vcPairAssetFolder);

            generateAppContext = EditorGUILayout.Toggle("Generate App Context", generateAppContext);
            appContextFolder = EditorGUILayout.TextField("Code folder", appContextFolder);
            appContextAssetFolder = EditorGUILayout.TextField("Asset folder", appContextAssetFolder);

            var viewAssetPath = AssetDatabase.GetAssetPath(sourceView);
            if (GUILayout.Button("Generate"))
            {
                var viewName = sourceView.name;
                Generate(
                    viewAssetPath,
                    viewName,
                    projectRootNamespace,
                    // view
                    generateUIView,
                    uiViewFolder,
                    // controller
                    generateUIController,
                    uiControllerFolder,
                    // pair
                    generateVCPair,
                    vcPairFolder,
                    vcPairAssetFolder,
                    // game context
                    generateAppContext,
                    appContextFolder,
                    appContextAssetFolder);
            }
            EditorGUILayout.Separator();
            if(GUILayout.Button("Save settings"))
            {
                SaveSettings();
            }
        }

        private static (string filename, string filenameGenerated) ViewFilename(string folder, string viewName)
        {
            var filename = Path.Combine(folder, viewName);
            return (filename + ".cs", filename + "_generated.cs");
        }

        private static (string controllerName, string filename) ControllerName(string folder, string viewName)
        {
            var controllerName = Regex.Replace(viewName, "view", string.Empty, RegexOptions.IgnoreCase) + "Controller";
            var filename = Path.Combine(folder, controllerName) + ".cs";
            return (controllerName, filename);
        }

        private static (string vcPairName, string filename, string assetFilename) UIPairName(string folder, string assetFolder, string viewName)
        {
            var vcPairName = Regex.Replace(viewName, "view", string.Empty, RegexOptions.IgnoreCase) + "Pair";
            var filename = Path.Combine(folder, vcPairName) + ".cs";
            var assetFilename = Path.Combine(assetFolder, vcPairName) + ".asset";
            return (vcPairName, filename, assetFilename);
        }

        private static (string appContextName, string filename, string assetFilename) AppContextName(string folder, string assetFolder, string viewName)
        {
            var appContextName = Regex.Replace(viewName, "uiview", string.Empty, RegexOptions.IgnoreCase) + "AppContext";
            if(Regex.IsMatch(appContextName,"view", RegexOptions.IgnoreCase))
            {
                // support for world views/contexts
                appContextName = Regex.Replace(viewName, "worldview", string.Empty, RegexOptions.IgnoreCase) + "AppContext";
            }
            var filename = Path.Combine(folder, appContextName) + ".cs";
            var assetFilename = Path.Combine(assetFolder, appContextName) + ".asset";
            return (appContextName, filename, assetFilename);
        }

        private static void Generate(
            string viewAssetPath,
            string viewName,
            string projectRootNamespace,
            // view
            bool generateUIView,
            string uiViewFolder,
            // controller
            bool generateUIController,
            string uiControllerFolder,
            // pair
            bool generateVCPair,
            string vcPairFolder,
            string vcPairAssetFolder,
            // app context
            bool generateAppContext,
            string appContextFolder,
            string appContextAssetFolder)
        {
            if (!string.IsNullOrEmpty(viewAssetPath))
            {
                var generators = GetGenerators();
                // generate UIView
                if (generateUIView)
                {
                    CorrectAssetEditing.EditUIAssetAtPath(viewAssetPath, (view) =>
                    {
                        var componentList = UIViewGenerator.BuildComponentList(view, generators);
                        var (filename, filenameGenerated) = ViewFilename(uiViewFolder, viewName);
                        UIViewGenerator.GenerateUIView(filename, filenameGenerated, viewName, projectRootNamespace, componentList, generators);
                    });
                }
                // add UIView to prefab
                if (generateUIView)
                {
                    CorrectAssetEditing.EditUIAssetAtPath(viewAssetPath, (view) =>
                    {
                        var viewComponent = UIViewGenerator.AddUIView(view, viewName, projectRootNamespace);
                        if (viewComponent != null)
                        {
                            var componentList = UIViewGenerator.BuildComponentList(view, generators);
                            UIViewGenerator.FillReferences(viewComponent, componentList, generators);
                        }
                        PrefabUtility.ApplyPrefabInstance(view, InteractionMode.AutomatedAction);
                    });
                    UIViewGenerator.MakeAddressable(viewAssetPath);
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
                }
                // generate UIController
                if (generateUIController)
                {
                    CorrectAssetEditing.EditUIAssetAtPath(viewAssetPath, (view) =>
                    {
                        string viewName = view.name;
                        var componentList = UIViewGenerator.BuildComponentList(view, generators);
                        var (controllerName, filename) = ControllerName(uiControllerFolder, viewName);
                        ControllerGenerator.GenerateController(filename, projectRootNamespace, controllerName, viewName, componentList, generators);
                    });
                }
                // generate UIPair
                if (generateVCPair)
                {
                    var guid = AssetDatabase.GUIDFromAssetPath(viewAssetPath);
                    var (controllerName, _) = ControllerName(uiControllerFolder, viewName);
                    var (uiPairName, uiPairFilename, assetFilename) = UIPairName(vcPairFolder, vcPairAssetFolder, viewName);
                    VCPairGenerator.GenerateVCPair(uiPairFilename, projectRootNamespace, uiPairName, controllerName, viewName);
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
                    VCPairGenerator.CreateUIPairAsset(uiPairName, projectRootNamespace, assetFilename, guid.ToString());
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
                }
                // generate app context
                if (generateAppContext)
                {
                    var (appContextName, filename, appContextAssetFilename) = AppContextName(appContextFolder, appContextAssetFolder, viewName);
                    AppContextGenerator.GenerateAppContext(filename, projectRootNamespace, appContextName);
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
                    var (uiPairName, uiPairFilename, uiPairAssetFilename) = UIPairName(vcPairFolder, vcPairAssetFolder, viewName);
                    var uiPairGUID = AssetDatabase.GUIDFromAssetPath(uiPairAssetFilename);
                    AppContextGenerator.CreateAppContextAsset(appContextName, projectRootNamespace, appContextAssetFilename, uiPairGUID.ToString());
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
                }
            }
            else
            {
                Debug.LogWarning($"No prefab at {viewAssetPath}");
            }
        }

        private class GameObjectComponent
        {
            public string codeName;
            public MonoBehaviour component;
        }

        private static List<ICodeGenerator> GetGenerators()
        {
            List<ICodeGenerator> generators = new List<ICodeGenerator>();
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (typeof(ICodeGenerator).IsAssignableFrom(type) && !type.IsAbstract && type.GetCustomAttributes(true).Any(a => a is CodeGenerationAttribute))
                {
                    ConstructorInfo constructor = type.GetConstructor(new Type[0]);
                    if (constructor != null)
                    {
                        var generator = constructor.Invoke(new object[0]);
                        generators.Add((ICodeGenerator)generator);
                    }
                }
            }
            return generators;
        }
    }
}
