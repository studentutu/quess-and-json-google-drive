using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Scripts.Services
{
    [Serializable]
    public class URLLoader : IServices, Entry.ISetUrl
    {
        [SerializeField] private string mainUrl = "";
        public string MainUrl => mainUrl;

        void Entry.ISetUrl.SetUrl(string newUrl)
        {
            mainUrl = newUrl;
        }

        /// <summary>
        /// Load all games data
        /// </summary>
        /// <param name="callback">success callback</param>
        /// <param name="error">error callback</param>
        public async System.Threading.Tasks.Task<T> LoadData<T>(
            IDisposableObject refToCheck,
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
                    await System.Threading.Tasks.Task.Yield(); // 0.005s
                }


                if (!IDisposableObject.IsValid(refToCheck) || req.isHttpError || req.isNetworkError)
                {
                    error.Invoke(req.error);
                    return default;
                }
                else
                {
                    return App.JsonConverter.FromJson<T>(req.downloadHandler.text);
                }
            }
        }

        /// <summary>
        /// Load all games data
        /// </summary>
        /// <param name="callback">success callback</param>
        /// <param name="error">error callback</param>
        public async System.Threading.Tasks.Task<string> LoadData(
            IDisposableObject refToCheck,
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
                    await System.Threading.Tasks.Task.Yield(); // 0.005s
                }


                if (!IDisposableObject.IsValid(refToCheck) || req.isHttpError || req.isNetworkError)
                {
                    error.Invoke(req.error);
                    return null;
                }
                else
                {
                    return req.downloadHandler.text;
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
