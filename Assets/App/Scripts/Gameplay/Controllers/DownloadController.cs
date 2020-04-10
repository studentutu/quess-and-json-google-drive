using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scripts.Gameplay.Models;
using Scripts.Gameplay.Views;
using UnityEngine;

namespace Scripts.Gameplay.Controllers
{
    public class DownloadController : IController
    {
        // Management of Textures
        private Dictionary<string, Texture2D> allTextures = null;

        private Task<bool> startedTask = null;
        private IDisposableObject disposableObject = null;
        void IController.Init()
        {
            App.ApplicationQuittingEvent -= OnQuit;
            App.ApplicationQuittingEvent += OnQuit;
            startedTask = LoadingAllContent();
        }

        public void InitializeView(GameFieldView gameField)
        {
            gameField.Initialize(allTextures);
        }

        private void OnQuit()
        {
            disposableObject?.Dispose();
            startedTask = null;
        }

        public async Task<bool> LoadingAllContent()
        {
            if (startedTask != null)
            {
                var result = await startedTask;
                startedTask = null;
                return result;
            }
            else
            {
                disposableObject = new IDisposableObject();
                var listOfItems = await App.WebLoader.LoadData(disposableObject,
                (msg) => { Debug.LogError(msg); },
                App.WebLoader.MainUrl
                );

                if (string.IsNullOrEmpty(listOfItems))
                {
                    Debug.LogWarning("string is null");
                    return false;
                }
                if (!IDisposableObject.IsValid(disposableObject))
                {
                    return false;
                }

                var parseTo = App.JsonConverter.FromJson<UrlsImageListModel>(listOfItems);
                if (allTextures == null)
                {
                    allTextures = new Dictionary<string, Texture2D>();
                }
                else
                {
                    foreach (var key in allTextures.Keys)
                    {
                        if (allTextures[key] != null)
                        {
                            Object.Destroy(allTextures[key]);
                        }
                    }
                }
                allTextures.Clear();

                for (int i = 0; i < parseTo.Urls.Count; i++)
                {
                    var tempTextureBase64 = await App.WebLoader.LoadData(new IDisposableObject(),
                        (msg) => { Debug.LogError(msg); },
                        parseTo.Urls[i].Url);

                    if (string.IsNullOrEmpty(tempTextureBase64))
                    {
                        Debug.LogWarning($" could not load {parseTo.Urls[i].Name}");
                        continue;
                    }
                    allTextures.Add(parseTo.Urls[i].Name, LoaderTextures.ParseToTexture(tempTextureBase64));
                }
                return true;
            }
        }
    }
}