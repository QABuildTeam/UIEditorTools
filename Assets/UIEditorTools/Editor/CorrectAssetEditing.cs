using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UIEditorTools
{
    public static class CorrectAssetEditing
    {
        public static bool EditUIAssetAtPath(string prefabAssetPath, Action<GameObject> prefabInstanceEditor)
        {
            var prefabView = (GameObject)AssetDatabase.LoadMainAssetAtPath(prefabAssetPath);
            if (prefabView == null)
            {
                Debug.LogError($"Could not load prefab at {prefabAssetPath}");
                return false;
            }
            if (prefabView.transform as RectTransform == null)
            {
                Debug.LogError($"No root RectTransform in prefab object at {prefabAssetPath}");
                return false;
            }

            // Dirty fix: PrefabUtility.ApplyPrefabInstance resets the root transform scale to Vector2.zero,
            // so save the original scale beforehand...
            var originalScale = prefabView.transform.localScale;
            // this is an instance of the prefab
            var view = (GameObject)PrefabUtility.InstantiatePrefab(prefabView);

            prefabInstanceEditor?.Invoke(view);
            // ...and restore before the actual saving the asset
            prefabView.transform.localScale = originalScale;
            PrefabUtility.SavePrefabAsset(prefabView);

            GameObject.DestroyImmediate(view);
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
            return true;
        }
    }
}
