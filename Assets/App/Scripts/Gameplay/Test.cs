using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private bool Testbool = false;
    [SerializeField] private int nextScene = 0;
    private void OnValidate()
    {
        if (Testbool)
        {
            Testbool = false;
            Scripts.App.SceneService.LoadSceneWithVideo(nextScene, null, 2);
        }
    }
}
