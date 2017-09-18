using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JGame
{
    public class UIManager_MainMenu : UIManager
    {
        #region UIFunc
        public void OnClick_Play()
        {
            SceneManager.LoadScene(Config.scene_InGame);
        }

        public void OnClick_Quit()
        {
            //{{@Test
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            //}}@Test
        }
        #endregion
    }
}
