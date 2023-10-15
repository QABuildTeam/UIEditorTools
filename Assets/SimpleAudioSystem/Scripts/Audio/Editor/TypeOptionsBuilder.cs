using System;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAudioSystem.Editor
{
    public static class TypeOptionsBuilder
    {
        private static void ScanTypeConstants(Type type, string typePath, Dictionary<string, int> optionDictionary)
        {
            if (!string.IsNullOrEmpty(typePath))
            {
                typePath += "/";
            }
            foreach (var field in type.GetFields().Where(f => f.IsPublic && f.FieldType == typeof(int) && f.IsLiteral && f.IsStatic))
            {
                var key = typePath + field.Name;
                if (!optionDictionary.ContainsKey(key))
                {
                    optionDictionary[key] = (int)field.GetRawConstantValue();
                }
                else
                {
                    Debug.LogWarning($"Key {key} already exists");
                }
            }
        }

        private static Dictionary<string, int> ScanNestedTypes(Type rootType)
        {
            var result = new Dictionary<string, int>();
            ScanTypeConstants(rootType, string.Empty, result);
            foreach (var nestedType in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t.FullName.Contains(rootType.FullName + "+")))
            {
                var nestedTypeName = nestedType.FullName.Substring(rootType.FullName.Length + 1);
                var typeNamePath = nestedTypeName.Replace("+", "/");
                ScanTypeConstants(nestedType, typeNamePath, result);
            }
            return result;
        }

        public static Dictionary<string, int> BuildMusicTypeOptions()
        {
            return ScanNestedTypes(typeof(MusicTrackType));
        }

        public static Dictionary<string, int> BuildSFXTypeOptions()
        {
            return ScanNestedTypes(typeof(SFXTrackType));
        }
    }
}
