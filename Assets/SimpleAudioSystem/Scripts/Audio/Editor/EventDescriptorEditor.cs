using System.Linq;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ACFW;

namespace SimpleAudioSystem.Editor
{
    public class EventDescriptorEditor : PropertyDrawer
    {
        private static Dictionary<string, List<string>> eventRealms;
        protected static Dictionary<string, List<string>> EventRealms
        {
            get
            {
                if (eventRealms == null)
                {
                    Type iept = typeof(IEventProvider);
                    eventRealms = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .Where(t => t != iept && iept.IsAssignableFrom(t))
                        .ToDictionary(
                            t => t.FullName.Replace(".", "/"),
                            t => t.GetFields()
                                .Where(f => f.FieldType.Name.StartsWith(nameof(UEvent)))
                                .Select(f => f.Name).ToList()
                                );
                }
                return eventRealms;
            }
        }

        private static List<string> eventRealmPopupElements;
        protected static List<string> EventRealmPopupElements
        {
            get
            {
                if (eventRealmPopupElements == null)
                {
                    eventRealmPopupElements = EventRealms.Keys.OrderBy(n => n).ToList();
                }
                return eventRealmPopupElements;
            }
        }

        private List<string> EventNamePopupElements(string eventRealmName)
        {
            if (EventRealms.ContainsKey(eventRealmName))
            {
                return EventRealms[eventRealmName].OrderBy(n => n).ToList();
            }
            return new List<string>();
        }

        private void StringPopup(Rect position, SerializedProperty property, List<string> options)
        {
            int selectedIndex = options.IndexOf(property.stringValue);
            var style = EditorStyles.popup;
            style.alignment = TextAnchor.MiddleRight;
            selectedIndex = EditorGUI.Popup(position, selectedIndex, options.ToArray(), style);
            property.stringValue = selectedIndex >= 0 && selectedIndex < options.Count ? options[selectedIndex] : string.Empty;
        }

        protected virtual void CreateTypePopup(Rect position, SerializedProperty property) { }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.DrawRect(position, Color.gray);

            float width = position.width / 3;

            position.x += 0.1f;
            position.width = width;
            SerializedProperty eventRealmProperty = property.FindPropertyRelative(nameof(EventDescriptor.eventRealm));
            StringPopup(position, eventRealmProperty, EventRealmPopupElements);

            position.x += width;
            SerializedProperty eventNameProperty = property.FindPropertyRelative(nameof(EventDescriptor.eventName));
            StringPopup(position, eventNameProperty, EventNamePopupElements(eventRealmProperty.stringValue));

            position.x += width;
            CreateTypePopup(position, property);

            EditorGUI.EndProperty();
        }
    }
}
