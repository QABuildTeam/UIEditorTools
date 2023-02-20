using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;
using SimpleAudioSystem.Settings;

namespace SimpleAudioSystem.Editor
{
    public partial class SimpleAudioSystemEditor
    {
        private static class MusicSFXTypesGenerator
        {
            public static (DefinitionTree<int>, int) InitialMusicTypesTree()
            {
                var tree = new DefinitionTree<int>();
                tree.AddLeaf(new string[] { nameof(MusicTrackType.None) }, MusicTrackType.None);
                tree.AddLeaf(new string[] { nameof(MusicTrackType.Universal) }, MusicTrackType.Universal);
                return (tree, 10);
            }

            public static (DefinitionTree<int>, int) InitialSFXTypesTree()
            {
                var tree = new DefinitionTree<int>();
                tree.AddLeaf(new string[] { nameof(SFXTrackType.Empty) }, SFXTrackType.Empty);
                tree.AddLeaf(new string[] { nameof(SFXTrackType.None) }, SFXTrackType.None);
                tree.AddLeaf(new string[] { nameof(SFXTrackType.Universal) }, SFXTrackType.Universal);
                return (tree, 10);
            }

            public static DefinitionTree<int> CreateMusicTree(AudioClipSettings settings)
            {
                var (tree, initialValue) = InitialMusicTypesTree();
                return DefinitionTreeBuilder.CompleteTree(tree, initialValue, settings.musicTracks);
            }

            public static DefinitionTree<int> CreateSFXTree(AudioClipSettings settings)
            {
                var (tree, initialValue) = InitialSFXTypesTree();
                return DefinitionTreeBuilder.CompleteTree(tree, initialValue, settings.sfxTracks);
            }

            private static string identDelta = "    ";
            private static string leafDefinitionTemplate = @"{0}public const int {1} = {2};
";
            private static string branchDefinitionHeaderTemplate = @"{0}public static class {1}
{0}{{
";
            private static string branchDefinitionFooterTemplate = @"
{0}}}
";

            private static string GenerateTreeDefinitions(DefinitionTree<int> currentTree, DefinitionTree<int> exceptTree, string ident)
            {
                string result = string.Empty;
                foreach (var leaf in currentTree.leaves.OrderBy(l => l.Value))
                {
                    if (exceptTree.GetPath(leaf.Value) == null)
                    {
                        result += string.Format(leafDefinitionTemplate, ident, leaf.Key, leaf.Value);
                    }
                }
                var nextIdent = ident + identDelta;
                foreach (var branch in currentTree.branches)
                {
                    result += string.Format(branchDefinitionHeaderTemplate, ident, branch.Key);
                    result += GenerateTreeDefinitions(branch.Value, exceptTree, nextIdent);
                    result += string.Format(branchDefinitionFooterTemplate, ident);
                }
                return result;
            }

            private static string musicTypesHeader = @"namespace SimpleAudioSystem
{
    public static partial class MusicTrackType
    {";
            private static string musicTypesFooter = @"
    }
}";
            private static string sfxTypesHeader = @"namespace SimpleAudioSystem
{
    public static partial class SFXTrackType
    {";
            private static string sfxTypesFooter = @"
    }
}";

            public static void GenerateMusicTypes(string filename, AudioClipSettings settings)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
                using (var stream = new StreamWriter(filename))
                {
                    var (exceptTree, index) = InitialMusicTypesTree();
                    stream.WriteLine(musicTypesHeader);
                    stream.WriteLine(GenerateTreeDefinitions(CreateMusicTree(settings), exceptTree, identDelta + identDelta));
                    stream.WriteLine(musicTypesFooter);
                }
                Debug.Log($"Created music types definitions at {filename}");
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
            }

            public static void GenerateSFXTypes(string filename, AudioClipSettings settings)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
                using (var stream = new StreamWriter(filename))
                {
                    var (exceptTree, index) = InitialSFXTypesTree();
                    stream.WriteLine(sfxTypesHeader);
                    stream.WriteLine(GenerateTreeDefinitions(CreateSFXTree(settings), exceptTree, identDelta + identDelta));
                    stream.WriteLine(sfxTypesFooter);
                }
                Debug.Log($"Created SFX types definitions at {filename}");
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
            }
        }
    }
}
