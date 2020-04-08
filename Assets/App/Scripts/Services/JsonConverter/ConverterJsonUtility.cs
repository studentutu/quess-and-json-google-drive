using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Scripts.Services
{
    [Serializable]
    public class ConverterJsonUtility : IConverter
    {
        public string SaveTo => saveTo;
        public string LoadFrom => loadFrom;

        [SerializeField] private string saveTo = "app.dat";
        [SerializeField] private string loadFrom = "url";

        public T FromJson<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        public string ToJson<T>(T toJson)
        {
            return JsonUtility.ToJson(toJson);
        }

        public T LoadData<T>(string fileWithExtension)
        {
            var path = Path.Combine(Application.persistentDataPath, fileWithExtension);
            return JsonUtility.FromJson<T>(
                Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(path))));
        }

        public void SaveFile(string fileWithExtension, object data)
        {
            var path = Path.Combine(Application.persistentDataPath, fileWithExtension);

            File.WriteAllText(path,
                Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data))));
        }
    }
}
