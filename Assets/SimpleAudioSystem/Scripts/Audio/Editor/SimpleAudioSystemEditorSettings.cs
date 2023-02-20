using UnityEngine;

namespace SimpleAudioSystem.Editor
{
    public class SimpleAudioSystemEditorSettings : ScriptableObject
    {
        public string projectRootNamespace = "SimpleAudioSystem";

        public string simpleAudioPlayerSettingsFilename = "Assets/SimpleAudioSystem/Settings/AudioSettings.asset";
        public string audioClipSettingsFilename = "Assets/SimpleAudioSystem/Settings/AudioClipSettings.asset";
        public string audioEventSettingsFilename = "Assets/SimpleAudioSystem/Settings/AudioEventSettings.asset";

        public string audioTypesPath = "Assets/SimpleAudioSystem/Scripts/Generated";
        public string musicTypesFilename = "MusicTrackType_generated.cs";
        public string sfxTypesFilename = "SFXTrackType_generated.cs";

        public string audioEventHandlersPath = "Assets/SimpleAudioSystem/Scripts/Generated";
        public string audioEventHandlersFilename = "AudioEventsHandler.cs";
    }
}
