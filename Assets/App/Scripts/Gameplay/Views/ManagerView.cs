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
            WaitForLoading();
        }

        private async void WaitForLoading()
        {
            var newController = new DownloadController();
            var isSuccessful = await newController.LoadingAllContent();

            if (isSuccessful != null)
            {
                gameField.Initialize(isSuccessful);
            }
        }
    }

}