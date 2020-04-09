using System.Collections;
using System.Collections.Generic;
using Scripts;
using Scripts.Services;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private bool Testbool = false;
    [SerializeField] private Texture2D convertTo = null;

    private void OnValidate()
    {
        if (Testbool)
        {
            Testbool = false;
            StartCoroutine(test());
        }
    }

    private IEnumerator test()
    {
        yield return null;
        var newString = LoaderTextures.ParseToBase64(convertTo);
        App.JsonConverter.SaveFile(App.JsonConverter.SaveFileWithExtension, newString);

        var asConverterJsonUtility = App.JsonConverter as ConverterJsonUtility;
        asConverterJsonUtility.WriteToLocalTextAsset(newString);

        // Debug.LogWarning(newString);
        // App.SceneService.LoadSceneWithVideo(nextScene, null, 2);

    }
}
