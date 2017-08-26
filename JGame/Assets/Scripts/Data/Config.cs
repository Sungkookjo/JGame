using UnityEngine;

namespace JGame
{
    public class Config
    {
        public const int MaxSoldierNum = 9; // 3x3

        // Team Index
        public const int teamNone = 255;
        public const int team1 = 0;
        public const int team2 = 1;

        public const int teamMax = 2;

        // system option
        public const float ResX = 1920.0f;
        public const float ResY = 1080.0f;

        public const float defaultResXY = ResX / ResY;
        public const float defaultResYX = ResY / ResX;

        // for data key
        public const string key_Country = "country";
        public const string key_DataVersion = "dataVersion";
    }

    public class JUtil
    {
        public static T FindComponent<T>() where T : MonoBehaviour
        {
            string objectName = typeof(T).Name;

            return FindComponent<T>(objectName);
        }

        public static T FindComponent<T>(string objectName) where T : MonoBehaviour
        {
            GameObject obj = GameObject.Find(objectName);
            if (obj == null)
            {
                return null;
            }

            T comp = obj.GetComponent<T>();

            return comp;
        }

        public static float GetMaxResRatio()
        {
            return Mathf.Max( GetResRatioX(), GetResRatioY() );
        }

        public static float GetMinResRatio()
        {
            return Mathf.Min(GetResRatioX(), GetResRatioY());
        }

        public static float GetResRatioX()
        {
            return (Screen.width / Config.ResX);
        }

        public static float GetResRatioY()
        {
            return (Screen.height / Config.ResY);
        }

        public static float GetResRatioXY()
        {
            return ((float)Screen.width / Screen.height);
        }

        public static float GetResRatioYX()
        {
            return ((float)Screen.height / Screen.width);
        }
    }
}