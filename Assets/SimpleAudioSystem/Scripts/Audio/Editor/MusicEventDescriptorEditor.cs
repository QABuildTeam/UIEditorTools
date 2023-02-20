using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SimpleAudioSystem.Editor
{
    [CustomPropertyDrawer(typeof(MusicEventDescriptor))]
    public class MusicEventDescriptorEditor : EventDescriptorEditor
    {
        private static Dictionary<string, int> musicOptions = TypeOptionsBuilder.BuildMusicTypeOptions();
        protected override void CreateTypePopup(Rect position, SerializedProperty property)
        {
            SerializedProperty musicTypeProperty = property.FindPropertyRelative(nameof(MusicEventDescriptor.musicType));
            var options = musicOptions.Keys.ToList();
            string musicName = musicOptions.FirstOrDefault(e => e.Value == musicTypeProperty.intValue).Key;
            int selectedIndex = options.IndexOf(musicName);
            selectedIndex = EditorGUI.Popup(position, selectedIndex, options.ToArray());
            if (selectedIndex >= 0 && selectedIndex < options.Count)
            {
                musicTypeProperty.intValue = musicOptions[options[selectedIndex]];
            }
        }
    }
}
