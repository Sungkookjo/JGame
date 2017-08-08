using System.Collections.Generic;
using UnityEngine;
using JGame.Data;

namespace JGame.Localization
{
    [System.Serializable]
    public class LocalizationData
    {
        public LocalizationItem[] items;
    }

    [System.Serializable]
    public class LocalizationItem
    {
        public string key;
        public string value;
    }

    public class LocalizationManager : MonoBehaviour
    {
        // instance
        private static LocalizationManager _instance = null;
        public static LocalizationManager instance
        {
            get
            {
                if (_instance == null) CreateInstance();

                return _instance;
            }
        }

        // localization data
        private Dictionary<string, string> localizedText;

        private bool isReady = false;
        private string missingTextString = "Localized text not found";
        
        protected static void CreateInstance()
        {
            if (instance != null) return;

            GameObject obj = new GameObject();

            if (obj != null)
            {
                obj.name = "Localization Manager";
                _instance = obj.AddComponent<LocalizationManager>();
                _instance.Initialize();
                DontDestroyOnLoad(obj);
            }
        }

        // Initialize
        public void Initialize()
        {
            LoadLocalizedText(GetCountry());
        }

        // load localization data file
        public void LoadLocalizedText(string fileName)
        {
            localizedText = new Dictionary<string, string>();
            string filePath = "Localization/" + fileName;

            // load data file in resource folder
            TextAsset targetFile = Resources.Load<TextAsset>(filePath);

            if (targetFile != null)
            {
                string dataAsJson = targetFile.text;

                LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);

                for (int i = 0; i < loadedData.items.Length; i++)
                {
                    localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
                }
            }
            else
            {
                Debug.LogError("Cannot find localization data file!");
            }

            isReady = true;
        }

        // get localized text
        public string GetLocalizedValue(string key)
        {
            string result = missingTextString;
            if (localizedText.ContainsKey(key))
            {
                result = localizedText[key];
            }

            return result;

        }

        // data file loaded?
        public bool GetIsReady()
        {
            return isReady;
        }

        // get country for localization
        public string GetCountry()
        {
            // from DataController
            if ( DataController.instance != null )
            {
                return DataController.instance.GetCountry();
            }

            // from PlayerPrefs
            if (!PlayerPrefs.HasKey(Config.key_Country))
            {
                string country;
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.Korean:
                        country = "Kor";
                        break;
                    default:
                        country = "Eng";
                        break;
                }
                PlayerPrefs.SetString(Config.key_Country, country);

                return country;
            }

            return PlayerPrefs.GetString(Config.key_Country, "Kor");
        }
    }
}
