using System.Collections;
using System.Collections.Generic;
using Scripts.Gameplay.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Gameplay.Views
{
    public class ManagerView : MonoBehaviour
    {
        [SerializeField] private GameFieldView gameField = null;
        [SerializeField] private Button reset = null;
        private void Awake()
        {
            reset.onClick.AddListener(Reset);
            gameField.Loading();
            var ifThereIsAController = App.GetController<DownloadController>();
            WaitForLoading(ifThereIsAController);
        }

        // not a good practice to use async void
        private async void WaitForLoading(DownloadController loadController)
        {
            var isSuccessful = await loadController.LoadingAllContent();
            if (isSuccessful)
            {
                loadController.InitializeView(gameField);
            }
        }

        private void Reset()
        {
            App.GetController<DownloadController>().InitializeView(gameField);
        }
    }

}