using System.Collections;
using System.Collections.Generic;
using Scripts.Services;
using UnityEngine;

public static class LoaderTextures
{
    /// <summary>
    /// Parse text in base64 string into Texture2D
    /// </summary>
    public static Texture2D ParseData(string initialBase64data)
    {
        string textureBase64 = TrimMetaDAta(initialBase64data);
        byte[] imageBytes = System.Convert.FromBase64String(textureBase64);
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(imageBytes);
        texture.Apply();
        return texture;
    }

    private static string TrimMetaDAta(string jsonBase64)
    {
        int indeOfComma = jsonBase64.IndexOf(',');
        var result = jsonBase64.Substring(indeOfComma + 1); // not include comma!
                                                            // result = result.Trim();
        return result;
    }
}
