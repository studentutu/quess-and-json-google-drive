using Scripts;
using Scripts.Async;
using Scripts.Services;
using Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entry : SingletonPersistent<Entry>
{
    public static event System.Action<IReadOnlyList<IServices>> InitializingServices;

    [SerializeField] private SceneManagementService sceneService = null;

    protected override void Awake()
    {
        base.Awake();

        ThreadTools.Initialize();

        List<IServices> currentServices = new List<IServices>
        {
            sceneService
        };
        App.Start();
        InitializingServices?.Invoke(currentServices);
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
