using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JGame.Data;

namespace JGame.Sound
{
    [System.Serializable]
    public enum SoundType
    {
        Effect,
        BGM,
        Max,
    }

    public class SoundManager : MonoBehaviour
    {
        #region instance
        static private SoundManager _instance = null;
        static public SoundManager instance
        {
            get
            {
                if (_instance == null) CreateInstance();

                return _instance;
            }
        }
        #endregion

        // sound volume. 0.0 ~ 1.0
        protected float[] volume = new float[(int)SoundType.Max];
        //protected bool[] isMute = new bool[(int)SoundType.Max];

        // create instance
        protected static void CreateInstance()
        {
            if (_instance != null) return;

            GameObject obj = new GameObject();

            if (obj != null)
            {
                obj.name = "Sound Manager";
                _instance = obj.AddComponent<SoundManager>();
                _instance.Initialize();
                DontDestroyOnLoad(obj);
            }
        }

        // initialize
        protected void Initialize()
        {
            foreach (SoundType type in System.Enum.GetValues(typeof(SoundType)))
            {
                SetVolume(type, DataController.instance.GetFloatData( VolumeTypeToKey(type) , 1.0f));
            }
        }

        public string VolumeTypeToKey( SoundType type )
        {
            return "Volume_" + type.ToString();
        }

        public float GetVolume( SoundType type )
        {
            //if( isMute[(int)type] )
            //{
            //    return 0.0f;
            //}

            return volume[(int)type];
        }

        public void SetVolume( SoundType type , float value )
        {
            int idx = (int)type;

            if (idx < 0 || idx >= volume.Length) return;

            if (volume[idx] == value) return;

            volume[idx] = Mathf.Clamp(value, 0.0f, 1.0f);
            
            DataController.instance.SetFloatData(VolumeTypeToKey(type), volume[idx]);
        }
        
    }
}