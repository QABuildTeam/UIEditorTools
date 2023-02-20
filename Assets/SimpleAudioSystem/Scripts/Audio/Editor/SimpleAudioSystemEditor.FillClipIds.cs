using UnityEngine;
using UnityEditor;
using SimpleAudioSystem.Settings;

namespace SimpleAudioSystem.Editor
{
    public partial class SimpleAudioSystemEditor
    {
        private static class ClipIdFiller
        {
            public static void FillIds(string assetFilename)
            {
                var asset = AssetDatabase.LoadAssetAtPath<AudioClipSettings>(assetFilename);
                var musicTree = MusicSFXTypesGenerator.CreateMusicTree(asset);
                foreach (var descriptor in asset.musicTracks)
                {
                    descriptor.numericType = musicTree.GetTreeLeaf(descriptor.descriptiveName);
                    Debug.Log($"Music track {descriptor.descriptiveName} = {descriptor.numericType}");
                }
                var sfxTree = MusicSFXTypesGenerator.CreateSFXTree(asset);
                foreach (var descriptor in asset.sfxTracks)
                {
                    descriptor.numericType = sfxTree.GetTreeLeaf(descriptor.descriptiveName);
                    Debug.Log($"SFX clip {descriptor.descriptiveName} = {descriptor.numericType}");
                }
                var guid = AssetDatabase.GUIDFromAssetPath(assetFilename);
                AssetDatabase.SaveAssetIfDirty(guid);
            }
        }
    }
}
