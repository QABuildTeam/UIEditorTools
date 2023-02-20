using System.Linq;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SimpleAudioSystem.Editor
{
    [CustomPropertyDrawer(typeof(SFXEventDescriptor))]
    public class SFXEventDescriptorEditor : EventDescriptorEditor
    {
        private static Dictionary<string, int> sfxOptions = TypeOptionsBuilder.BuildSFXTypeOptions();
        protected override void CreateTypePopup(Rect position, SerializedProperty property)
        {
            SerializedProperty sfxTypeProperty = property.FindPropertyRelative(nameof(SFXEventDescriptor.sfxType));
            var options = sfxOptions.Keys.ToList();
            string sfxName = sfxOptions.FirstOrDefault(e => e.Value == sfxTypeProperty.intValue).Key;
            int selectedIndex = options.IndexOf(sfxName);
            selectedIndex = EditorGUI.Popup(position, selectedIndex, options.ToArray());
            if (selectedIndex >= 0 && selectedIndex < options.Count)
            {
                sfxTypeProperty.intValue = sfxOptions[options[selectedIndex]];
            }
        }
    }
}
