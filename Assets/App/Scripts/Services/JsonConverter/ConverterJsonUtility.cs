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
        public string SaveFileWithExtension => saveTo;

        [SerializeField] private string saveTo = "app.custom";
        [SerializeField] private TextAsset saveToAsset = null;

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
            var path = Application.persistentDataPath + "/" + fileWithExtension;
            return JsonUtility.FromJson<T>(
                Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(path))));
        }

        public void SaveFile(string fileWithExtension, object data)
        {
            var path = Application.persistentDataPath + "/" + fileWithExtension;
            Debug.LogWarning("path : " + path);
            File.WriteAllText(path,
                Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data))));
        }

        public void WriteToLocalTextAsset(string fullString)
        {
            string path = UnityEditor.AssetDatabase.GetAssetPath(saveToAsset);
            int first = path.IndexOf('/');
            if (first == -1)
            {
                first = path.IndexOf('\\');
            }
            Debug.LogWarning(path);
            path = path.Substring(first + 1);
            path = Application.dataPath + "/" + path;
            Debug.LogWarning(path);

            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning("Please specify the Asset to write into");
                return;
            }

            File.WriteAllText(path, fullString);
            UnityEditor.EditorUtility.SetDirty(saveToAsset);
            try
            {
                UnityEditor.AssetDatabase.SaveAssets();
            }
            catch (System.Exception)
            {
            }
        }
    }
}
