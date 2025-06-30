using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class ScreenshotCapturer : MonoBehaviour
{
    public Camera captureCamera; 
    public string screenshotName = "screenshot.png";

    public int width = 1920;
    public int height = 1080;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        RenderTexture rt = new RenderTexture(width, height, 24);
        captureCamera.targetTexture = rt;

        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        captureCamera.Render();
        RenderTexture.active = rt;

        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();

        captureCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        string filename = Path.Combine(Application.dataPath, screenshotName);
        File.WriteAllBytes(filename, bytes);

        Debug.Log($"Screenshot guardado en: {filename}");
    }
}

