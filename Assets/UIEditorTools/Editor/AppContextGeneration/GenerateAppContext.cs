using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ACFW.Controllers;

namespace UIEditorTools.Editor
{
    public partial class AppContextGenerationUtility : EditorWindow
    {
        private static class AppContextGenerator
        {
            private static string appContextTemplate = @"
namespace {1}.Controllers
{{
    public class {0} : AppContext
    {{
    }}
}}";
            private static List<string> internalUsingClauses = new List<string>
        {
            "ACFW.Controllers"
        };
            public static void GenerateAppContext(string filename, string projectRootNamespace, string appContextName)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
                Debug.Log($"Creating app context script {filename}");
                using (var stream = new StreamWriter(filename))
                {
                    var usingClauses = new GenerateUsingClauses(internalUsingClauses);
                    stream.Write(usingClauses.GetUsingClauses());
                    stream.WriteLine(string.Format(appContextTemplate, appContextName, projectRootNamespace));
                }
            }

            public static void CreateAppContextAsset(string appContextName, string projectRootNamespace, string assetFilename, string vcPairGUID)
            {
                var appContextType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.FullName.EndsWith($"{projectRootNamespace}.Controllers.{appContextName}"));
                if (appContextType != null)
                {
                    var vcPairPath = AssetDatabase.GUIDToAssetPath(vcPairGUID);
                    var vcPair = AssetDatabase.LoadAssetAtPath(vcPairPath, typeof(ScriptableReference)) as ScriptableReference;
                    if (vcPair != null)
                    {
                        bool isUI = vcPair.GetType().Name.Contains("UI");
                        var asset = AssetDatabase.LoadAssetAtPath(assetFilename, appContextType);
                        if (asset == null)
                        {
                            // create new asset
                            var appContext = ScriptableObject.CreateInstance(appContextType) as ACFW.Controllers.AppContext;
                            if (appContext != null)
                            {
                                Debug.Log($"vcPair={vcPair}");
                                if (isUI)
                                {
                                    appContext.uiObjects = new ScriptableReference[] { vcPair };
                                }
                                else
                                {
                                    appContext.worldObjects = new ScriptableReference[] { vcPair };
                                }
                                Debug.Log($"Created {appContextType} asset in {assetFilename}");
                                AssetDatabase.CreateAsset(appContext, assetFilename);
                            }
                            else
                            {
                                Debug.LogWarning($"Could not create object {appContextType.Name}");
                                return;
                            }
                        }
                        else
                        {
                            // re-create existing asset
                            Debug.Log($"App context {appContextType.Name} asset at {assetFilename} already exists");
                            var appContext = asset as ACFW.Controllers.AppContext;
                            List<ScriptableReference> worldObjects= appContext.worldObjects.ToList();
                            List<ScriptableReference> uiObjects= appContext.uiObjects.ToList();
                            List<ScriptableReference> pairObjects = isUI ? uiObjects : worldObjects;
                            var hasUIPair = pairObjects.Any(v => v.GetType().Equals(vcPair.GetType()));
                            if (!hasUIPair)
                            {
                                pairObjects.Add(vcPair);
                                Debug.Log($"Added VCPair {vcPair.GetType().Name} to existing app context {appContextType.Name} at {assetFilename}");
                            }
                            else
                            {
                                Debug.Log($"Existing app context asset {appContextType.Name} at {assetFilename} already has VCPair {vcPair.GetType().Name}");
                            }
                            appContext = ScriptableObject.CreateInstance(appContextType) as ACFW.Controllers.AppContext;
                            if (appContext != null)
                            {
                                appContext.worldObjects = worldObjects.ToArray();
                                appContext.uiObjects = uiObjects.ToArray();
                                Debug.Log($"worldObjects are {string.Join(",", worldObjects.Select(o => o.GetType().Name))}");
                                Debug.Log($"uiObjects are {string.Join(",", uiObjects.Select(o => o.GetType().Name))}");
                                AssetDatabase.CreateAsset(appContext, assetFilename);
                                Debug.Log($"Re-created {appContextType} asset at {assetFilename}");
                            }
                            else
                            {
                                Debug.LogWarning($"Could not create object {appContextType.Name}");
                                return;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Could not find VCPair asset at {vcPairPath} (GUID {vcPairGUID})");
                    }
                    return;
                }
                else
                {
                    Debug.LogWarning($"Could not find {appContextType} type");
                }
            }
        }
    }
}
