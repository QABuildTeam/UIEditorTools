using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

namespace UIEditorTools.Editor
{
    public class ImageSizeRounding : EditorWindow
    {
        [MenuItem("Tools/UI Editor Tools/Image processing/Image size rounding", false, 0)]
        private static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(ImageSizeRounding));
        }

        private string sourceDirectory = "Assets/UIEditorTools/ResourceData/Images";

        private void OnGUI()
        {
            GUILayout.Label("Resize images to match x4 constraint");
            sourceDirectory = EditorGUILayout.TextField("Source directory", sourceDirectory);
            if (GUILayout.Button("Process images"))
            {
                var assetPaths = ScanImages(sourceDirectory);
                foreach (var assetPath in assetPaths)
                {
                    Debug.Log($"Found {assetPath}");
                    ProcessImage(assetPath);
                }
                Debug.Log($"Refreshing AssetDatabase");
                AssetDatabase.Refresh();
                Debug.Log($"Total {assetPaths.Length} assets");
            }
        }

        private static string[] ScanImages(string directory)
        {
            var assets = AssetDatabase.FindAssets("t:Texture", new string[] { directory }).Select(a => AssetDatabase.GUIDToAssetPath(a)).ToArray();
            return assets;
        }

        private static List<TextureImporterPlatformSettings> GetTexturePlatformSettings()
        {

            List<TextureImporterPlatformSettings> platforms = new List<TextureImporterPlatformSettings>();
            // Android Settings
            platforms.Add(new TextureImporterPlatformSettings());
            platforms[0].name = "Android";
            platforms[0].overridden = true;
            platforms[0].maxTextureSize = 8192; // some textures are THAT big
            platforms[0].resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            platforms[0].format = TextureImporterFormat.ETC2_RGBA8Crunched;
            platforms[0].compressionQuality = 50;
            platforms[0].androidETC2FallbackOverride = AndroidETC2FallbackOverride.UseBuildSettings;
            // iOS Settings
            platforms.Add(new TextureImporterPlatformSettings());
            platforms[1].name = "iPhone";
            platforms[1].overridden = true;
            platforms[1].maxTextureSize = 8192;
            platforms[1].resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            platforms[1].format = TextureImporterFormat.ETC2_RGBA8Crunched;
            platforms[1].compressionQuality = 50;

            return platforms;
        }

        private static void ProcessImage(string assetPath)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.isReadable = true;
                importer.mipmapEnabled = false;
                importer.alphaIsTransparency = true;
                importer.SaveAndReimport();
                Debug.Log($"Changed import settings: isReadable->{importer.isReadable}");

                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                var textureWidth = texture.width % 4 == 0 ? texture.width : (texture.width / 4 + 1) * 4;
                var textureHeight = texture.height % 4 == 0 ? texture.height : (texture.height / 4 + 1) * 4;
                Debug.Log($"Checking asset {assetPath}: width: {texture.width}->{textureWidth}, height: {texture.height}->{textureHeight}");

                if (textureWidth != texture.width || textureHeight != texture.height)
                {

                    texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                    var newTexture = new Texture2D(textureWidth, textureHeight);
                    // clear the new texture
                    int pixelLength = textureWidth * textureHeight;
                    Color32[] pixels = new Color32[pixelLength];
                    Color32 blank = new Color32 { a = 0, r = 0, g = 0, b = 0 };
                    for (int i = 0; i < pixelLength; ++i)
                    {
                        pixels[i] = blank;
                    }
                    newTexture.SetPixels32(0, 0, textureWidth, textureHeight, pixels);
                    int xStart = (textureWidth - texture.width) / 2;
                    int yStart = (textureHeight - texture.height) / 2;
                    // copy original texture into the center of the new texture
                    pixels = texture.GetPixels32();
                    Debug.Log($"Read {pixels.Length} pixels from {assetPath}");
                    if (pixels.Length > 0)
                    {
                        newTexture.SetPixels32(xStart, yStart, texture.width, texture.height, pixels);
                        newTexture.Apply();
                        File.WriteAllBytes(assetPath, newTexture.EncodeToPNG());
                        Debug.Log($"Saved to {assetPath}");
                    }
                }
                Debug.Log($"Change platform compression settings for {assetPath}");
                foreach (var setting in GetTexturePlatformSettings())
                {
                    importer.SetPlatformTextureSettings(setting);
                }
                importer.SaveAndReimport();
                AssetDatabase.ImportAsset(assetPath);
                Debug.Log($"Reimported {assetPath}");
            }
        }
    }
}
