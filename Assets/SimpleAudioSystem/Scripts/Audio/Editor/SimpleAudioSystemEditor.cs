using System.IO;
using UnityEngine;
using UnityEditor;
using SimpleAudioSystem.Settings;

namespace SimpleAudioSystem.Editor
{
    public partial class SimpleAudioSystemEditor : EditorWindow
    {
        [MenuItem("Tools/Audio/Simple Audio System")]
        private static void ShowWIndow()
        {
            EditorWindow.GetWindow(typeof(SimpleAudioSystemEditor));
        }

        private string simpleAudioPlayerSettingsFilename = "Assets/SimpleAudioSystem/Settings/AudioSettings.asset";
        private bool simpleAudioPlayerSettingsOverwrite = false;

        private string audioClipSettingsFilename = "Assets/SimpleAudioSystem/Settings/AudioClipSettings.asset";
        private bool audioClipSettingsOverwrite = false;

        private string audioEventSettingsFilename = "Assets/SimpleAudioSystem/Settings/AudioEventSettings.asset";
        private bool audioEventSettingsOverwrite = false;

        private string audioTypesPath = "Assets/SimpleAudioSystem/Scripts/Generated";
        private string musicTypesFilename = "MusicTrackType_generated.cs";
        private string sfxTypesFilename = "SFXTrackType_generated.cs";

        private AudioClipSettings audioClipSettingsAsset;
        private string audioEventHandlersPath = "Assets/SimpleAudioSystem/Scripts/Generated";
        private string audioEventHandlersFilename = "AudioEventsSubscriber_generated.cs";

        private AudioEventSettings audioEventSettingsAsset;
        private void OnGUI()
        {


            // SimpleAudioPlayer settings
            simpleAudioPlayerSettingsFilename = EditorGUILayout.TextField("Audio settings file name", simpleAudioPlayerSettingsFilename);
            simpleAudioPlayerSettingsOverwrite = EditorGUILayout.Toggle("Force re-create", simpleAudioPlayerSettingsOverwrite);
            if (GUILayout.Button("Create audio player settings"))
            {
                SettingsCreator.CreateSimpleAudioPlayerSettings(simpleAudioPlayerSettingsFilename, simpleAudioPlayerSettingsOverwrite);
            }
            GUILayout.Space(20);

            // AudioClipSettings
            audioClipSettingsFilename = EditorGUILayout.TextField("Audio clip settings file name", audioClipSettingsFilename);
            audioClipSettingsOverwrite = EditorGUILayout.Toggle("Force re-create", audioClipSettingsOverwrite);
            if (GUILayout.Button("Create audio clip settings"))
            {
                SettingsCreator.CreateAudioClipSettings(audioClipSettingsFilename, audioEventSettingsOverwrite);
            }
            GUILayout.Space(20);

            // AudioEventSettings
            audioEventSettingsFilename = EditorGUILayout.TextField("Audio event settings file name", audioEventSettingsFilename);
            audioEventSettingsOverwrite = EditorGUILayout.Toggle("Force re-create", audioEventSettingsOverwrite);
            if (GUILayout.Button("Create audio event settings"))
            {
                SettingsCreator.CreateAudioEventSettings(audioEventSettingsFilename, audioEventSettingsOverwrite);
            }
            GUILayout.Space(20);

            // Music and SFX types definitions
            if (File.Exists(audioClipSettingsFilename))
            {
                audioClipSettingsAsset = AssetDatabase.LoadAssetAtPath<AudioClipSettings>(audioClipSettingsFilename);
                audioClipSettingsAsset = EditorGUILayout.ObjectField("Audio clip settings", audioClipSettingsAsset, typeof(AudioClipSettings), true) as AudioClipSettings;
                audioTypesPath = EditorGUILayout.TextField("Music and SFX types path", audioTypesPath);
                musicTypesFilename = EditorGUILayout.TextField("Music types generated script filename", musicTypesFilename);
                sfxTypesFilename = EditorGUILayout.TextField("SFX types generated script filename", sfxTypesFilename);
                if (GUILayout.Button("Fill in clip ids and generate scripts with music and SFX types"))
                {
                    var settings = audioClipSettingsAsset;
                    var musicTypesPathname = Path.Combine(audioTypesPath, musicTypesFilename);
                    MusicSFXTypesGenerator.GenerateMusicTypes(musicTypesPathname, settings);
                    var sfxTypesPathname = Path.Combine(audioTypesPath, sfxTypesFilename);
                    MusicSFXTypesGenerator.GenerateSFXTypes(sfxTypesPathname, settings);
                    ClipIdFiller.FillIds(audioClipSettingsFilename);
                }
                GUILayout.Space(20);

                // Audio event subscribers
                if (File.Exists(audioEventSettingsFilename))
                {
                    audioEventSettingsAsset = AssetDatabase.LoadAssetAtPath<AudioEventSettings>(audioEventSettingsFilename);
                    audioEventSettingsAsset = EditorGUILayout.ObjectField("Audio event settings", audioEventSettingsAsset, typeof(AudioEventSettings), true) as AudioEventSettings;
                    audioEventHandlersPath = EditorGUILayout.TextField("Audio event subscribers script path", audioEventHandlersPath);
                    audioEventHandlersFilename = EditorGUILayout.TextField("Audio event subscribers script filename", audioEventHandlersFilename);
                    if (GUILayout.Button("Generate scipt with audio event handlers"))
                    {
                        EventHandlersGenerator.Generate(Path.Combine(audioEventHandlersPath, audioEventHandlersFilename), audioClipSettingsAsset, audioEventSettingsAsset);
                    }
                }
            }
        }
    }
}
