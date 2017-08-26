using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    public class UIPanel_Ingame_SelectStatus : UIPanel
    {
        GameObject selection;

        public void SetSelection( GameObject obj )
        {
            if (obj != null)
            {
                Tile t = obj.GetComponent<Tile>();

                if (t != null)
                {
                    obj = t.actor;
                }
            }

            if (selection == obj) return;

            selection = obj;

            if(selection != null )
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        protected override void InitFromAwake()
        {
            base.InitFromAwake();
            UIManager_InGame mgr = (UIManager_InGame)UIManager.instance;

            if( mgr != null )
            {
                mgr.selectedObjStat = this;
            }
        }

        protected override void InitFromStart()
        {
            base.InitFromStart();
            gameObject.SetActive(false);
        }
    }
}