using System.Collections;
using System.Collections.Generic;
using Scripts;
using Scripts.Services;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] private bool Testbool = false;
    [SerializeField] private Texture2D convertTo = null;
    [SerializeField] private RawImage imageTOPassIn = null;

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
        // asConverterJsonUtility.WriteToLocalTextAsset(newString);

        // Debug.LogWarning(newString);
        // App.SceneService.LoadSceneWithVideo(nextScene, null, 2);

        // var texture = LoaderTextures.ParseToTexture(asConverterJsonUtility.ReadAllFromTExtAsset());

        // imageTOPassIn.texture = texture;
        runAsync();
    }

    private async void runAsync()
    {
        var DisposableObject = new IDisposableObject();
        var yieldFor = await App.WebLoader.LoadData(DisposableObject, (msg) =>
       {
           Debug.LogError(" Error " + msg);
       },
        "https://drive.google.com/uc?export=download&id=13XUhCO_BR5RA9j7-7zI_8BOEGeyBQvpZ" //+ "/pub?output=txt"
        );
        // Debug.LogWarning(yieldFor);
        var texture = LoaderTextures.ParseToTexture(yieldFor);
        imageTOPassIn.texture = texture;

    }
}
