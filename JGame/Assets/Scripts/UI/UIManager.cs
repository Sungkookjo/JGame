using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    public enum UIWindow
    {
        Command,
        Status_Selected,
        Status_Hero,
    }

    public class UIManager : MonoBehaviour
    {
        protected List<UIObject> uiObjects = new List<UIObject>();
        protected static UIManager _instance = null;
        public static UIManager instance {  get { return _instance; } }

        private void Awake()
        {
            _instance = this;
        }

        private void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        public void AddUIObject( UIObject ui )
        {
            if( uiObjects.Find( item => item == ui ) != null )
            {
                return;
            }

            uiObjects.Add(ui);
        }

        public void RemoveUIObject( UIObject ui )
        {
            uiObjects.Remove(ui);
        }

        public virtual void ShowWindow(UIWindow wnd, int param)
        {

        }

        public virtual void CloseWindow(UIWindow wnd)
        {
            
        }

        public virtual void SetSelection( UIWindow wnd, GameObject selection )
        {

        }
    }
}
