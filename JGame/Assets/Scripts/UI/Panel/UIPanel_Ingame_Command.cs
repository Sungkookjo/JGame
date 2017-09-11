using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JGame
{
    public enum UIWnd_Cmd
    {
        Attack,
        Move,
    }

    public enum UICmd_Attack
    {
        Cancel,
        Attack,
        Throw,
    }

    public enum UICmd_Move
    {
        Cancel,
        Move,
        Rotate,
    }

    public class UIPanel_Ingame_Command : UIPanel
    {
        protected int selectedBtIndex;

        public Button attCmd_Att;
        public Button attCmd_Throw;
        public Button attCmd_Cancel;
        public Button moveCmd_Move;
        public Button moveCmd_Rotate;
        public Button moveCmd_Cancel;

        public GameObject attackCommand;
        public GameObject moveCommand;
        
        protected override void InitFromStart()
        {
            UIManager_InGame mgr = (UIManager_InGame)UIManager.instance;

            if (mgr != null)
            {
                mgr.commandMenu = this;
            }

            base.InitFromStart();

            SetActive(false , true);
            SetShowAllWindows(false);

            // attack command - attack
            if (attCmd_Att != null)
            {
                attCmd_Att.onClick.AddListener(() => { selectedBtIndex = (int)UICmd_Attack.Attack; });
            }

            // attack command - throw
            if (attCmd_Throw != null)
            {
                attCmd_Throw.onClick.AddListener(() => { selectedBtIndex = (int)UICmd_Attack.Throw; });
            }

            // attack command - cancel
            if (attCmd_Cancel != null)
            {
                attCmd_Cancel.onClick.AddListener(() => { selectedBtIndex = (int)UICmd_Attack.Cancel; });
            }

            // move command - move
            if (moveCmd_Move != null)
            {
                moveCmd_Move.onClick.AddListener(() => { selectedBtIndex = (int)UICmd_Move.Move; });
            }

            // move command - rotate
            if (moveCmd_Rotate != null)
            {
                moveCmd_Rotate.onClick.AddListener(() => { selectedBtIndex = (int)UICmd_Move.Rotate; });
            }

            // move command - cancel
            if (moveCmd_Cancel != null)
            {
                moveCmd_Cancel.onClick.AddListener(() => { selectedBtIndex = (int)UICmd_Move.Cancel; });
            }
        }

        public IEnumerator ShowWindow( int param1 )
        {
            selectedBtIndex = -1;

            SetActive(true);
            SetShowAllWindows(false);

            ShowWindow((UIWnd_Cmd)param1);
            
            while(selectedBtIndex < 0 )
            {
                if( !gameObject.activeInHierarchy )
                {
                    selectedBtIndex = 0;
                }

                yield return null;
            }

            SetActive(false);

            yield return selectedBtIndex;

        }

        protected void SetShowAllWindows(bool bShow)
        {
            if (attackCommand != null) attackCommand.SetActive(bShow);
            if (moveCommand != null) moveCommand.SetActive(bShow);
        }

        protected void ShowWindow( UIWnd_Cmd type )
        {
            switch(type)
            {
                case UIWnd_Cmd.Attack:
                    if (attackCommand != null)
                    {
                        UpdateAttackCmdInteractable();
                        attackCommand.SetActive(true);
                    }
                    break;
                case UIWnd_Cmd.Move:
                    if (moveCommand != null)
                    {
                        UpdateMoveCmdInteractable();
                        moveCommand.SetActive(true);
                    }
                    break;
            }
        }

        protected void UpdateAttackCmdInteractable()
        {
            // attack command - attack
            if (attCmd_Att != null)
            {
                attCmd_Att.interactable = GameManager.instance.localController.CanAttackSelection();
            }

            // attack command - throw
            if (attCmd_Throw != null)
            {
                attCmd_Throw.interactable = GameManager.instance.localController.CanThrowSelection();
            }
        }

        protected void UpdateMoveCmdInteractable()
        {
            // move command - move
            if (moveCmd_Move != null)
            {
                moveCmd_Move.interactable = GameManager.instance.localController.CanMoveSelection();
            }
        }
    }
}