using Scripts;
using Scripts.Async;
using Scripts.Services;
using Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecutionOrder(-1000)]
public class Entry : SingletonSelfCreator<Entry>
{
    [Tooltip("Don't forget to pass it into the Resources folder!")]
    [SerializeField] private string resourcesRelativePath = "";
    [SerializeField] private SceneManagementService sceneService = null;
    [SerializeField] private ConverterJsonUtility jsonConverter = null;
    [SerializeField] private URLLoader webLoader = null;

    protected override string prefabPath => resourcesRelativePath;
    private bool isInitialized = false;
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
        App.Start(currentServices);
        Debug.LogWarning(" Started");
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
