using System;
using UnityEngine;
using UnityEditor;

namespace UIEditorTools
{
    public class MakeAdaptiveUILayoutUtility : EditorWindow
    {
        [MenuItem("Tools/UI Editor Tools/Make Adaptive UI", false, 0)]
        private static void ShowWIndow()
        {
            EditorWindow.GetWindow(typeof(MakeAdaptiveUILayoutUtility));
        }

        private GameObject sourceView;
        private Vector2 referenceScreenSize = new Vector2(1920, 1080);
        private void OnGUI()
        {
            GUILayout.Label("Make an adaptive layout");

            sourceView = EditorGUILayout.ObjectField("Source view prefab", sourceView, typeof(GameObject), true) as GameObject;
            referenceScreenSize = EditorGUILayout.Vector2Field("Reference screen size", referenceScreenSize);

            if (GUILayout.Button("Adapt"))
            {
                if (sourceView != null)
                {
                    var viewAssetPath = AssetDatabase.GetAssetPath(sourceView);
                    if (viewAssetPath != null)
                    {
                        LoadAndProcess(viewAssetPath);
                    }
                }
            }
        }

        private void LoadAndProcess(string viewAssetPath)
        {
            CorrectAssetEditing.EditUIAssetAtPath(viewAssetPath, (view) =>
            {
                var rootTransform = view.transform as RectTransform;
                ProcessRoot(rootTransform);
                PrefabUtility.ApplyPrefabInstance(view, InteractionMode.UserAction);
            });
        }

        private void ProcessRoot(RectTransform rootTransform)
        {
            ProcessTransform(rootTransform, (rt) => rt.name != "ScreenBackground");
        }

        private void ProcessTransform(RectTransform rt, Func<RectTransform, bool> includeFunc)
        {
            var childCount = rt.childCount;
            for (int i = 0; i < childCount; ++i)
            {
                var child = rt.GetChild(i) as RectTransform;
                if (child != null && includeFunc(child))
                {
                    if (includeFunc(child))
                    {
                        RecalculateTransformAnchors(rt, child);
                    }
                    ProcessTransform(child, includeFunc);
                }
            }
        }

        private void RecalculateTransformAnchors(RectTransform parent, RectTransform rt)
        {
            if (rt.drivenByObject)
            {
                return;
            }
            var parentRTsize = parent.rect.size;
            var rect = rt.rect;
            var anchorInParent = (Vector2.Scale(rt.anchorMin, parentRTsize) + Vector2.Scale(rt.anchorMax, parentRTsize)) / 2;
            var anchoredPosition = rt.anchoredPosition;
            var parentLocalPosition = anchorInParent + anchoredPosition;
            var anchorMin = new Vector2((parentLocalPosition + rect.min).x / parentRTsize.x, (parentLocalPosition + rect.min).y / parentRTsize.y);
            var anchorMax = new Vector2((parentLocalPosition + rect.max).x / parentRTsize.x, (parentLocalPosition + rect.max).y / parentRTsize.y);
            /*
            Debug.Log(
                $"Processing object {rt.name}: " +
                $"parent.rect.size={parentRTsize}, " +
                $"rect.min={rect.min}, " +
                $"rect.max={rect.max}, " +
                $"rect.size={rect.size}, " +
                $"anchoredPosition={anchoredPosition}, " +
                $"anchorInParent={anchorInParent}, " +
                $"parentLocalPosition={parentLocalPosition}, " +
                $"anchorMin={anchorMin}, " +
                $"anchorMax={anchorMax}");
            */
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.ForceUpdateRectTransforms();
        }
    }
}
