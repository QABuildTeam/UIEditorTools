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
    public partial class GameContextGenerationUtility : EditorWindow
    {

        [MenuItem("Tools/UI Editor Tools/Code generation/Generate Game Context", false, 0)]
        private static void ShowWIndow()
        {
            EditorWindow.GetWindow<GameContextGenerationUtility>().LoadSettings();
        }

        private string projectRootNamespace = "ACFW.Example";

        private GameObject sourceView;

        private string uiViewFolder = "Assets/AppContextFramework/Example/Scripts/Contexts/Generated/Views";
        private string uiControllerFolder = "Assets/AppContextFramework/Example/Scripts/Contexts/Generated/ContextControllers";
        private string uiPairFolder = "Assets/AppContextFramework/Example/Scripts/Contexts/Generated/ViewControllerPairs";
        private string uiPairAssetFolder = "Assets/AppContextFramework/Example/Settings/GameContexts";
        private string gameContextFolder = "Assets/AppContextFramework/Example/Scripts/Contexts/Generated/GameContexts";
        private string gameContextAssetFolder = "Assets/AppContextFramework/Example/Settings/GameContexts";

        private bool generateUIView = true;
        private bool generateUIController = true;
        private bool generateUIPair = true;
        private bool generateGameContext = true;

        private static string gameContextGenerationSettingsAssetPath = "Assets/UIEditorTools/Resources/GameContextGenerationSettings.asset";

        private GameContextGenerationSettings CreateSettings()
        {
            var settings = ScriptableObject.CreateInstance<GameContextGenerationSettings>();
            settings.projectRootNamespace = projectRootNamespace;
            settings.uiViewFolder = uiViewFolder;
            settings.uiControllerFolder = uiControllerFolder;
            settings.uiPairFolder = uiPairFolder;
            settings.uiPairAssetFolder = uiPairAssetFolder;
            settings.gameContextFolder = gameContextFolder;
            settings.gameContextAssetFolder = gameContextAssetFolder;
            return settings;
        }
        private GameContextGenerationUtility LoadSettings()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(gameContextGenerationSettingsAssetPath));
            GameContextGenerationSettings settings = AssetDatabase.LoadAssetAtPath<GameContextGenerationSettings>(gameContextGenerationSettingsAssetPath);
            if (settings == null)
            {
                settings = CreateSettings();
                AssetDatabase.CreateAsset(settings, gameContextGenerationSettingsAssetPath);
            }
            projectRootNamespace = settings.projectRootNamespace;
            uiViewFolder = settings.uiViewFolder;
            uiControllerFolder = settings.uiControllerFolder;
            uiPairFolder = settings.uiPairFolder;
            uiPairAssetFolder = settings.uiPairAssetFolder;
            gameContextFolder = settings.gameContextFolder;
            gameContextAssetFolder = settings.gameContextAssetFolder;
            return this;
        }

        private void SaveSettings()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(gameContextGenerationSettingsAssetPath));
            var settings = CreateSettings();
            AssetDatabase.CreateAsset(settings, gameContextGenerationSettingsAssetPath);
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
        }

        private void OnGUI()
        {
            GUILayout.Label("Generate game context from a UI view");

            sourceView = EditorGUILayout.ObjectField("Source view prefab", sourceView, typeof(GameObject), true) as GameObject;

            projectRootNamespace= EditorGUILayout.TextField("Project root namespace", projectRootNamespace);

            generateUIView = EditorGUILayout.Toggle("Generate UI View", generateUIView);
            uiViewFolder = EditorGUILayout.TextField("To folder", uiViewFolder);

            generateUIController = EditorGUILayout.Toggle("Generate UI Controller", generateUIController);
            uiControllerFolder = EditorGUILayout.TextField("To folder", uiControllerFolder);

            generateUIPair = EditorGUILayout.Toggle("Generate UI Pair", generateUIPair);
            uiPairFolder = EditorGUILayout.TextField("To folder", uiPairFolder);
            uiPairAssetFolder = EditorGUILayout.TextField("Asset folder", uiPairAssetFolder);

            generateGameContext = EditorGUILayout.Toggle("Generate UI Context", generateGameContext);
            gameContextFolder = EditorGUILayout.TextField("To folder", gameContextFolder);
            gameContextAssetFolder = EditorGUILayout.TextField("Asset folder", gameContextAssetFolder);

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
                    generateUIPair,
                    uiPairFolder,
                    uiPairAssetFolder,
                    // game context
                    generateGameContext,
                    gameContextFolder,
                    gameContextAssetFolder);
            }
            EditorGUILayout.Separator();
            if(GUILayout.Button("Save settings"))
            {
                SaveSettings();
            }
        }

        private static (string filename, string filenameGenerated) UIViewFilename(string folder, string uiViewName)
        {
            var filename = Path.Combine(folder, uiViewName);
            return (filename + ".cs", filename + "_generated.cs");
        }

        private static (string controllerName, string filename) UIControllerName(string folder, string uiViewName)
        {
            var controllerName = Regex.Replace(uiViewName, "uiview", string.Empty, RegexOptions.IgnoreCase) + "UIController";
            var filename = Path.Combine(folder, controllerName) + ".cs";
            return (controllerName, filename);
        }

        private static (string uiPairName, string filename, string assetFilename) UIPairName(string folder, string assetFolder, string uiViewName)
        {
            var uiPairName= Regex.Replace(uiViewName, "uiview", string.Empty, RegexOptions.IgnoreCase) + "UIPair";
            var filename = Path.Combine(folder, uiPairName) + ".cs";
            var assetFilename = Path.Combine(assetFolder, uiPairName) + ".asset";
            return (uiPairName, filename, assetFilename);
        }

        private static (string gameContextName, string filename, string assetFilename) GameContextName(string folder, string assetFolder, string uiViewName)
        {
            var gameContextName = Regex.Replace(uiViewName, "uiview", string.Empty, RegexOptions.IgnoreCase) + "GameContext";
            var filename = Path.Combine(folder, gameContextName) + ".cs";
            var assetFilename = Path.Combine(assetFolder, gameContextName) + ".asset";
            return (gameContextName, filename, assetFilename);
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
            bool generateUIPair,
            string uiPairFolder,
            string uiPairAssetFolder,
            // game context
            bool generateGameContext, string gameContextFolder, string gameContextAssetFolder)
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
                        var (filename, filenameGenerated) = UIViewFilename(uiViewFolder, viewName);
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
                        var (controllerName, filename) = UIControllerName(uiControllerFolder, viewName);
                        UIControllerGenerator.GenerateUIController(filename, projectRootNamespace, controllerName, viewName, componentList, generators);
                    });
                }
                // generate UIPair
                if (generateUIPair)
                {
                    var guid = AssetDatabase.GUIDFromAssetPath(viewAssetPath);
                    var (controllerName, _) = UIControllerName(uiControllerFolder, viewName);
                    var (uiPairName, uiPairFilename, assetFilename) = UIPairName(uiPairFolder, uiPairAssetFolder, viewName);
                    UIPairGenerator.GenerateUIPair(uiPairFilename, projectRootNamespace, uiPairName, controllerName, viewName);
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
                    UIPairGenerator.CreateUIPairAsset(uiPairName, projectRootNamespace, assetFilename, guid.ToString());
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
                }
                // generate game context
                if (generateGameContext)
                {
                    var (gameContextName, filename, gameContextAssetFilename) = GameContextName(gameContextFolder, gameContextAssetFolder, viewName);
                    GenerateGameContext(filename, projectRootNamespace, gameContextName);
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
                    var (uiPairName, uiPairFilename, uiPairAssetFilename) = UIPairName(uiPairFolder, uiPairAssetFolder, viewName);
                    var uiPairGUID = AssetDatabase.GUIDFromAssetPath(uiPairAssetFilename);
                    CreateGameContextAsset(gameContextName, projectRootNamespace, gameContextAssetFilename, uiPairGUID.ToString());
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
