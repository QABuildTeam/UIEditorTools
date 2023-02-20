using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using SimpleAudioSystem.Settings;

namespace SimpleAudioSystem.Editor
{
    public partial class SimpleAudioSystemEditor : EditorWindow
    {
        private static class SettingsCreator
        {
            private static void CreateSettings<T>(string pathname, bool forceOverwrite, Func<T> creator, string messageFormat) where T : UnityEngine.Object
            {
                if (!File.Exists(pathname) || forceOverwrite)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(pathname));
                    var settings = creator();
                    AssetDatabase.CreateAsset(settings, pathname);
                    Debug.Log(string.Format(messageFormat, pathname));
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
                }
            }
            public static void CreateSimpleAudioPlayerSettings(string pathname, bool forceOverwrite)
            {
                CreateSettings<AudioSettings>(
                    pathname,
                    forceOverwrite,
                    () =>
                    {
                        var settings = ScriptableObject.CreateInstance(typeof(AudioSettings)) as AudioSettings;
                        settings.data = new SimpleAudioPlayerSettings();
                        return settings;
                    },
                    "Created simple audio player settings {0}");
            }
            public static void CreateAudioClipSettings(string pathname, bool forceOverwrite)
            {
                CreateSettings<AudioClipSettings>(
                    pathname,
                    forceOverwrite,
                    () => ScriptableObject.CreateInstance(typeof(AudioClipSettings)) as AudioClipSettings,
                    "Created audio clip settings {0}");
            }
            public static void CreateAudioEventSettings(string pathname, bool forceOverwrite)
            {
                CreateSettings<AudioEventSettings>(
                    pathname,
                    forceOverwrite,
                    () => ScriptableObject.CreateInstance(typeof(AudioEventSettings)) as AudioEventSettings,
                    "Created audio event settings {0}");
            }
        }
    }
}
