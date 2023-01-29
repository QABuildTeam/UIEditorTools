using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UIEditorTools.Controllers;

namespace UIEditorTools.Editor
{
    public partial class GameContextGenerationUtility : EditorWindow
    {
        private static string gameContextTemplate = @"using UnityEngine;

namespace {1}.Settings
{{
    public class {0} : GameContext
    {{
    }}
}}";
        private static void GenerateGameContext(string filename, string projectRootNamespace, string gameContextName)
        {
            if (!File.Exists(filename))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
                Debug.Log($"Creating game context script {filename}");
                using (var stream = new StreamWriter(filename))
                {
                    stream.WriteLine(string.Format(gameContextTemplate, gameContextName, projectRootNamespace));
                }
            }
            else
            {
                Debug.Log($"Skipping game context script {filename} - already exists");
            }
        }

        private static void CreateGameContextAsset(string gameContextName, string projectRootNamespace, string assetFilename, string uiPairGUID)
        {
            var gameContextType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.FullName.EndsWith($"{projectRootNamespace}.Settings.{gameContextName}"));
            if (gameContextType != null)
            {
                var uiPairPath = AssetDatabase.GUIDToAssetPath(uiPairGUID);
                var uiPair = AssetDatabase.LoadAssetAtPath(uiPairPath, typeof(ViewControllerPair)) as ViewControllerPair;
                if (uiPair != null)
                {
                    var asset = AssetDatabase.LoadAssetAtPath(assetFilename, gameContextType);
                    if (asset == null)
                    {
                        // create new asset
                        var gameContext = ScriptableObject.CreateInstance(gameContextType) as GameContext;
                        if (gameContext != null)
                        {
                            gameContext.uiObjects = new ViewControllerPair[] { uiPair };
                            Debug.Log($"Created {gameContextType} asset in {assetFilename}");
                            AssetDatabase.CreateAsset(gameContext, assetFilename);
                        }
                        else
                        {
                            Debug.LogWarning($"Could not create object {gameContextType.Name}");
                            return;
                        }
                    }
                    else
                    {
                        Debug.Log($"Game context {gameContextType.Name} asset at {assetFilename} already exists");
                        //return;
                        var gameContext = asset as GameContext;
                        List<ViewControllerPair> uiObjects = gameContext.uiObjects.ToList();
                        var hasUIPair = uiObjects.Any(v => v.GetType().Equals(uiPair.GetType()));
                        if (!hasUIPair)
                        {
                            uiObjects.Add(uiPair);
                            Debug.Log($"Added UIPair {uiPair.GetType().Name} to existing game context {gameContextType.Name} at {assetFilename}");
                        }
                        else
                        {
                            Debug.Log($"Existing game context asset {gameContextType.Name} at {assetFilename} already has UIPair {uiPair.GetType().Name}");
                        }
                        gameContext = ScriptableObject.CreateInstance(gameContextType) as GameContext;
                        if (gameContext != null)
                        {
                            Debug.Log($"uiObjects are {string.Join(",", uiObjects.Select(o => o.GetType().Name))}");
                            gameContext.uiObjects = uiObjects.ToArray();
                            Debug.Log($"Re-created {gameContextType} asset in {assetFilename}");
                            AssetDatabase.CreateAsset(gameContext, assetFilename);
                        }
                        else
                        {
                            Debug.LogWarning($"Could not create object {gameContextType.Name}");
                            return;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"Could not find uiPair asset at {uiPairPath} (GUID {uiPairGUID})");
                }
                return;
            }
            else
            {
                Debug.LogWarning($"Could not find {gameContextType} type");
            }
        }
    }
}
