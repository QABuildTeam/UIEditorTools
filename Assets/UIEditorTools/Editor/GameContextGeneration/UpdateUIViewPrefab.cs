using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace UIEditorTools.Editor
{
    public partial class GameContextGenerationUtility : EditorWindow
    {
        private static partial class UIViewGenerator
        {
            public static MonoBehaviour AddUIView(GameObject view, string viewName, string projectRootNamespace)
            {
                var viewType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.FullName.EndsWith($"{projectRootNamespace}.Views.{viewName}"));
                if (viewType == null)
                {
                    Debug.LogWarning($"Could not find {viewName} type");
                    return null;
                }
                MethodInfo getComponentMethod = typeof(GameObject)
                    .GetMethods()
                    .FirstOrDefault(m => m.Name.Equals(nameof(GameObject.GetComponent))
                        && m.IsGenericMethod
                        && m.GetGenericArguments().Length == 1)?
                    .MakeGenericMethod(viewType);
                if (getComponentMethod == null)
                {
                    Debug.LogWarning($"Could not find {nameof(GameObject.GetComponent)} method");
                    return null;
                }
                var result = getComponentMethod.Invoke(view, new object[] { });
                if (result != null)
                {
                    return result as MonoBehaviour;
                }
                MethodInfo addComponentMethod = typeof(GameObject)
                    .GetMethods()
                    .FirstOrDefault(m => m.Name.Equals(nameof(GameObject.AddComponent))
                        && m.IsGenericMethod
                        && m.GetGenericArguments().Length == 1)?
                    .MakeGenericMethod(viewType);
                if (addComponentMethod == null)
                {
                    Debug.LogWarning($"Could not find {nameof(GameObject.AddComponent)} method");
                    return null;
                }
                addComponentMethod.Invoke(view, new object[] { });
                result = getComponentMethod.Invoke(view, new object[] { });
                if (result != null)
                {
                    return result as MonoBehaviour;
                }
                Debug.LogWarning($"No component of type {viewType.Name} found");
                return null;
            }

            public static void FillReferences(MonoBehaviour component, List<GameObjectComponent> components, List<ICodeGenerator> generators)
            {
                foreach (var field in component.GetType().GetFields(BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Instance).Where(f => f.GetCustomAttributes(true).Any(a => a is SerializeField)))
                {
                    var componentField = components.FirstOrDefault(c => c.codeName.Equals(field.Name));
                    if (componentField != null)
                    {
                        if (generators.Any(g => !string.IsNullOrEmpty(g.ComponentControlNameCore(componentField.codeName, componentField.component))))
                        {
                            field.SetValue(component, componentField.component);
                        }
                    }
                }
            }

            public static void MakeAddressable(string assetPath)
            {
                var settings = AddressableAssetSettingsDefaultObject.Settings;

                if (settings != null)
                {
                    var group = settings.DefaultGroup;

                    var guid = AssetDatabase.AssetPathToGUID(assetPath);

                    if (!group.entries.Any(e => e.guid.Equals(guid)))
                    {
                        var e = settings.CreateOrMoveEntry(guid, group, false, false);
                        var entriesAdded = new List<AddressableAssetEntry> { e };
                        group.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, false, true);
                        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true, false);
                        Debug.Log($"Made asset {assetPath} addressable with GUID {guid}");
                    }
                }
            }
        }
    }
}
