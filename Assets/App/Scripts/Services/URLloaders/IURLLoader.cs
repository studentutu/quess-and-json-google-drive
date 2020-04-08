using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Scripts.Services
{
    [Serializable]
    public class URLLoader : IServices
    {
        [SerializeField] private string mainUrl = "";
        public string MainUrl => mainUrl;

        /// <summary>
        /// Load all games data
        /// </summary>
        /// <param name="callback">success callback</param>
        /// <param name="error">error callback</param>
        public async void LoadGamesData<T>(
            IDisposableObject refToCheck,
            System.Action<T, bool> callback,
            System.Action<string> error,
            string url)
        {
            using (var req = UnityWebRequest.Get(url))
            {
#pragma warning disable
                req.SendWebRequest();
#pragma warning restore
                while (!req.isDone && !req.isHttpError && !req.isNetworkError && IDisposableObject.IsValid(refToCheck))
                {
                    await System.Threading.Tasks.Task.Delay(5); // 0.005s
                }
                if (!IDisposableObject.IsValid(refToCheck))
                {
                    // Debug.LogWarning(" Cancelled!");
                    return;
                }

                if (req.isHttpError || req.isNetworkError)
                {
                    error.Invoke(req.error);
                }
                else
                {
                    ParseData(req.downloadHandler.text, callback, error);
                }

            }

        }

        /// <summary>
        /// Parse json string into Game struct
        /// </summary>
        private static void ParseData<T>(string json, System.Action<T, bool> callback, System.Action<string> error)
        {
            if (!string.IsNullOrEmpty(json))
            {
                var data = App.JsonConverter.FromJson<T>(json);
                callback?.Invoke(data, true);
            }
            else
            {
                callback?.Invoke(default, false);
            }
        }
    }
}
