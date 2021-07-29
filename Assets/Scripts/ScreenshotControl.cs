using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public class ScreenshotControl : MonoBehaviour
{
    public int scaling = 8;
    public bool toggleScreenshot;

    void Update()
    {
        if (toggleScreenshot) {
            toggleScreenshot = false;
            var targetPath = EditorUtility.SaveFilePanelInProject("Render Path", "render", "png", "File to render to.");
            if (!string.IsNullOrWhiteSpace(targetPath))
                typeof(ScreenCapture).GetMethod("CaptureScreenshot",
                    BindingFlags.Static | BindingFlags.NonPublic,
                    null,
                    new [] {typeof(string), typeof(int), typeof(ScreenCapture.StereoScreenCaptureMode)},
                    new ParameterModifier[3]
                ).Invoke(null, new object[] {targetPath, scaling, ScreenCapture.StereoScreenCaptureMode.LeftEye});
        }
    }
}
