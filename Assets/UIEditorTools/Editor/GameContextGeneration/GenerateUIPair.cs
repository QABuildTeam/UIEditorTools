using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;

namespace UIEditorTools.Editor
{
    public partial class GameContextGenerationUtility : EditorWindow
    {
        private static string uiPairTemlpate = @"
namespace {0}.Settings
{{
    public class {1} : ViewControllerPair<{2}, {3}>
    {{
        protected override {2} GetContextController({3} view, UniversalEnvironment environment)
        {{
            return new {2}(view, environment);
        }}
    }}
}}";
        private static List<string> uiPairInternalUsingClauses = new List<string>
        {
            "UnityEngine",
            "UIEditorTools",
            "UIEditorTools.Environment",
            "UIEditorTools.Settings"
        };
        private static void GenerateUIPair(string scriptFilename, string projectRootNamespace, string uiPairName, string uiControllerName, string uiViewName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(scriptFilename));
            Debug.Log($"Creating UIPair script {scriptFilename}");
            using (var stream = new StreamWriter(scriptFilename))
            {
                var usingClauses = new GenerateUsingClauses(uiPairInternalUsingClauses);
                usingClauses.Add(string.Format("{0}.Controllers", projectRootNamespace));
                usingClauses.Add(string.Format("{0}.Views", projectRootNamespace));
                stream.Write(usingClauses.GetUsingClauses());
                stream.WriteLine(string.Format(uiPairTemlpate, projectRootNamespace, uiPairName, uiControllerName, uiViewName));
            }
        }

        private static void CreateUIPairAsset(string uiPairName, string projectRootNamespace, string assetFilename, string uiViewGUID)
        {
            var pairType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.FullName.EndsWith($"{projectRootNamespace}.Settings.{uiPairName}"));
            if (pairType != null)
            {
                var pair = AssetDatabase.LoadAssetAtPath(assetFilename, pairType);
                if (pair == null)
                {
                    pair = ScriptableObject.CreateInstance(pairType);
                    if (pair != null)
                    {
                        // since ViewControllerPair.viewAssetReference is only visible from within its base classes, scan it there
                        var baseType = pairType.BaseType;
                        while (baseType != null)
                        {
                            var assetReference = baseType
                                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default)
                                .FirstOrDefault(f => f.FieldType.Equals(typeof(AssetReference)) || f.FieldType.IsSubclassOf(typeof(AssetReference)));
                            if (assetReference != null)
                            {
                                assetReference.SetValue(pair, new AssetReference(uiViewGUID));
                                AssetDatabase.CreateAsset(pair, assetFilename);
                                Debug.Log($"Created {pairType} asset in {assetFilename}");
                                return;
                            }
                            baseType = baseType.BaseType;
                        }
                        Debug.LogWarning($"Could not find field {pairType.Name}.viewAssetReference");
                    }
                    else
                    {
                        Debug.LogWarning($"Could not create object {pairType.Name}");
                    }
                }
                else
                {
                    Debug.Log($"UIPair {pairType} asset at {assetFilename} already exists");
                }
            }
            else
            {
                Debug.LogWarning($"Could not find {pairType} type");
            }
        }
    }
}
