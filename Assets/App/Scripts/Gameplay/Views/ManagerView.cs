using System.Collections;
using System.Collections.Generic;
using Scripts.Gameplay.Controllers;
using UnityEngine;

namespace Scripts.Gameplay.Views
{
    public class ManagerView : MonoBehaviour
    {
        [SerializeField] private GameFieldView gameField = null;

        private void Awake()
        {
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
    }

}