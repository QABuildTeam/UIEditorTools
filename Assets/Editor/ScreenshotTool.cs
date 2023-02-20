using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace MagicChain.Editor
{
    public class ScreenshotTool : EditorWindow
    {

        [MenuItem("Tools/Capture screenshot")]
        private static void CaptureScreenShot()
        {
            string productPrefix = "MC";
#if UNITY_IOS
            string platformPrefix = "iOS";
#elif UNITY_ANDROID
            string platformPrefix = "Android";
#else
            string platformPrefix = "Generic";
#endif
            string screenshotPathname = Path.Combine(Application.dataPath, "..", "..", "Screenshots");
            if (!Directory.Exists(screenshotPathname))
            {
                Directory.CreateDirectory(screenshotPathname);
            }
            string screenSizePrefix = Screen.width.ToString() + "x" + Screen.height.ToString();

            int screenshotNumber = 0;
            string fileMask = $"{productPrefix}_{platformPrefix}_{screenSizePrefix}_screenshot_*.png";
            string[] files = Directory.GetFiles(screenshotPathname, fileMask, SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                Match matches = Regex.Match(file, @"_screenshot_(\d{6})\.png");
                if (matches.Groups.Count > 1)
                {
                    if (int.TryParse(matches.Groups[1].Value, out int number))
                    {
                        screenshotNumber = number + 1;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            string filename = $"{productPrefix}_{platformPrefix}_{screenSizePrefix}_screenshot_{screenshotNumber:000000}.png";
            string screenshotFilename = Path.Combine(screenshotPathname, filename);
            ScreenCapture.CaptureScreenshot(screenshotFilename);
        }
    }
}
