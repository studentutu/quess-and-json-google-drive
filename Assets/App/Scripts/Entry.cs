using Scripts;
using Scripts.Utils.Async;
using Scripts.Services;
using Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using Scripts.Gameplay.Controllers;
using UnityEngine;

[ExecutionOrder(-1000)]
public class Entry : SingletonSelfCreator<Entry>
{
    public interface ISetUrl
    {
        void SetUrl(string newUrl);
    }

    [Tooltip("Don't forget to pass it into the Resources folder!")]
    [SerializeField] private string resourcesRelativePath = "";
    [SerializeField] private SceneManagementService sceneService = null;
    [SerializeField] private ConverterJsonUtility jsonConverter = null;
    [SerializeField] private URLLoader webLoader = null;

    protected override string PrefabPath => resourcesRelativePath;
    // protected override bool IsDontDestroy => false;
    [System.NonSerialized] private bool isInitialized = false;

    private void OnValidate()
    {
        if (webLoader != null && !string.IsNullOrEmpty(webLoader.MainUrl))
        {
            // parse
            var asSeturl = webLoader as ISetUrl;
            asSeturl.SetUrl(webLoader.MainUrl.Replace("open?id", "uc?export=download&id"));
        }
    }

    public void Init()
    {
        if (isInitialized)
        {
            return;
        }
        isInitialized = true;
        ThreadTools.Initialize();

        List<IServices> currentServices = new List<IServices>
        {
            sceneService,
            jsonConverter,
            webLoader
        };

        List<IController> controllers = new List<IController>{
            new DownloadController(),
        };
        App.Start(currentServices, controllers);
        // Init only after the App starts
        foreach (var item in controllers)
        {
            item.Init();
        }
        App.SceneService.LoadSceneWithVideo(1, null, 2);
    }

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    protected override void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        App.Quit();
    }

    private void OnApplicationPause(bool value)
    {
        App.Pause(value);
    }
}
