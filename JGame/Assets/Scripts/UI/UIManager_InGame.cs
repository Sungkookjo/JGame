using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    public class UIManager_InGame : UIManager
    {
        public GameObject NormalHUD;
        public GameObject BattleHUD;
        public GameObject MessageUI;
        public GameObject OptionWnd;

        public UIPanel_Ingame_Command commandMenu;
        public UIPanel_Ingame_SelectStatus selectedObjStat;
        public UIPanel_Ingame_SelectStatus curHeroStat;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override IEnumerator ShowWindow(UIWindow wnd, int param1)
        {
            IEnumerator e = null;

            switch(wnd)
            {
                case UIWindow.Command:
                    if( commandMenu != null)
                    {
                        e = commandMenu.ShowWindow(param1);
                    }
                    break;
            }

            if (e == null)
            {
#if UNITY_EDITOR
                Debug.LogError("UIManager_Ingame::ShowWindow e == null. wnd = "+wnd);
#endif
                yield return base.ShowWindow(wnd, param1);
            }
            else
            {
                object result = null;

                while( e.MoveNext() )
                {
                    result = e.Current;

                    yield return result;
                }
            }
        }

        public override void CloseWindow(UIWindow wnd)
        {
            switch (wnd)
            {
                case UIWindow.Command:
                    if (commandMenu != null)
                    {
                        commandMenu.SetActive(false);
                    }
                    break;
                default:
                    base.CloseWindow(wnd);
                    break;
            }
        }

        public override void SetSelection(UIWindow wnd, GameObject selection)
        {
            switch (wnd)
            {
                case UIWindow.Status_Hero:
                    if (curHeroStat != null)
                    {
                        curHeroStat.SetSelection(selection);
                    }
                    break;
                case UIWindow.Status_Selected:
                    if (selectedObjStat != null)
                    {
                        selectedObjStat.SetSelection(selection);
                    }
                    break;
                default:
                    base.SetSelection(wnd, selection);
                    break;
            }
        }

        public void SetShowOptionWnd(bool bShow)
        {
            if(OptionWnd != null)
            {
                OptionWnd.SetActive(bShow);
            }
        }

        protected IEnumerator MessageBox_Surrender()
        {
            if( MessageUI == null )
            {
                yield break;
            }
            
            var wnd = MessageUI.GetComponent<UIPanel_MessageBox_YN>();

            if (wnd != null)
            {
                wnd.ShowMsgBox("L_Ask_Surrender");

                while ( wnd.IsWorking() )
                {
                    yield return new WaitForSeconds(0.1f);
                }

                if (wnd.IsYes())
                {
                    GameManager.instance.Surrender();
                }
            }
        }

        #region UIFunc
        public void OnClick_PassTurn()
        {
            GameManager.instance.localController.PassCurrentHeroTurn();
        }

        public void OnClick_EndTurn()
        {
            GameManager.instance.localController.EndCurrentHeroTurn();
        }

        public void OnClick_Option()
        {
            SetShowOptionWnd(true);
        }

        public void OnClick_Surrender()
        {
            StartCoroutine(MessageBox_Surrender());
            SetShowOptionWnd(false);
        }
        #endregion
    }
}
