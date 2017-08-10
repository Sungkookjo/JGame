using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame.Data
{
    public class DataController : MonoBehaviour
    {
        // data version
        const float version = 1.0f;

        // instance
        private static DataController _instance = null;
        public static DataController instance
        {
            get
            {
                if (_instance == null) CreateInstance();

                return _instance;
            }
        }

        protected static void CreateInstance()
        {
            if (_instance != null) return;

            GameObject obj = new GameObject();

            if (obj != null)
            {
                obj.name = "Data Controller";
                _instance = obj.AddComponent<DataController>();
                _instance.Initialize();
                DontDestroyOnLoad(obj);
            }
        }

        // check data version
        private bool IsOldData()
        {
            return (GetFloatData(Config.key_DataVersion) < version);
        }

        // update old data
        private void UpdateOldData()
        {
            SetFloatData(Config.key_DataVersion, version);
        }

        private bool IsFirstExcute()
        {
            return !PlayerPrefs.HasKey(Config.key_DataVersion);
        }

        private void DoFirstExcute()
        {
            SetFloatData(Config.key_DataVersion, version);
        }

        // initialize
        protected void Initialize()
        {
            //@Test
            //PlayerPrefs.DeleteAll();

            if (IsFirstExcute())
            {
                DoFirstExcute();
            }

            if (IsOldData())
            {
                UpdateOldData();
            }
        }

        // load Json data
        public static T LoadJson<T>(string path) where T : class
        {
            TextAsset targetFile = Resources.Load<TextAsset>(path);

            T data = null;

            if (targetFile != null)
            {
                string dataAsJson = targetFile.text;

                data = JsonUtility.FromJson<T>(dataAsJson);
            }
            else
            {
                Debug.LogError("Cannot find data file!");
            }

            return data;
        }

        // get local country
        public string GetCountry()
        {
            if (!PlayerPrefs.HasKey(Config.key_Country))
            {
                string Country;
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.Korean:
                        Country = "Kor";
                        break;
                    default:
                        Country = "Eng";
                        break;
                }
                SetStrData(Config.key_Country, Country);
            }
            return GetStrData(Config.key_Country, "Kor");
        }
        
        #region get/set int,float,string data
        public string GetStrData(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public int GetIntData(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public float GetFloatData(string key, float defaultValue = 0.0f)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public void SetStrData(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public void SetIntData(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public void SetFloatData(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }
        #endregion
    }
}
