using System.Collections;
using System.Collections.Generic;
using Scripts;
using Scripts.Gameplay.Models;
using Scripts.Services;
using Scripts.Utils.Async;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] private bool WriteImageToBase = false;
    [SerializeField] private Texture2D convertTo = null;
    [SerializeField] private RawImage imageTOPassIn = null;
    [SerializeField] private TextAsset saveToBase64 = null;


    [SerializeField] private bool WriteUrls = false;
    [SerializeField] private UrlsImageListModel modelUrlLists = null;
    [SerializeField] private TextAsset saveTo = null;

    private void OnValidate()
    {
        if (WriteImageToBase)
        {
            WriteImageToBase = false;
            ThreadTools.StartCoroutine(writeAsBase64());
        }

        if (WriteUrls)
        {
            WriteUrls = false;
            ThreadTools.StartCoroutine(writToCor());
        }

        foreach (var item in modelUrlLists.Urls)
        {
            if (!string.IsNullOrEmpty(item.Url))
            {
                // parse
                item.Url = item.Url.Replace("open?id", "uc?export=download&id");
            }
        }
    }

    private IEnumerator writToCor()
    {
        yield return null;
        var asConverterJsonUtility = App.JsonConverter as ConverterJsonUtility;
        asConverterJsonUtility.WriteToLocalTextAsset(App.JsonConverter.ToJson(modelUrlLists), saveTo);
    }

    private IEnumerator writeAsBase64()
    {
        yield return null;
        var newString = LoaderTextures.ParseToBase64(convertTo);

        var asConverterJsonUtility = App.JsonConverter as ConverterJsonUtility;
        asConverterJsonUtility.WriteToLocalTextAsset(newString, saveToBase64);
        // Debug.LogWarning(newString);
        // App.SceneService.LoadSceneWithVideo(nextScene, null, 2);

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
        if (imageTOPassIn != null)
        {
            imageTOPassIn.texture = texture;
        }

    }
}
