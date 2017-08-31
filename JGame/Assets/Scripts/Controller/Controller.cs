using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JGame.Pool;

namespace JGame
{
    public class Controller : Actor
    {
        // cached object when button down
        GameObject btDownObj = null;

        // cached object last selected
        GameObject selectedObj = null;

        // selected tile arlram
        GameObject arlramObj = null;

        bool lockSelect = false;

        Hero curHero = null;
        protected bool _passHeroTurn;

        protected bool _isTurnEnded;
        // heros
        protected List<GameObject> teamHeros = new List<GameObject>();

        int curHeroIndex;

        // is auto?
        public bool isAuto = false;

        // change team
        public override void SetTeam(Team newTeam)
        {
            base.SetTeam(newTeam);

            if (team == newTeam) return;

            for (int i = 0; i < teamHeros.Count; ++i)
            {
                var hero = teamHeros[i].GetComponent<Hero>();

                if (hero != null)
                {
                    hero.SetTeam(team);
                }
            }
        }

        // begin turn
        public void BeginTurn()
        {
            SetSelectedObject(null);

            curHeroIndex = 0;
            curHero = null;

            for (int i = 0; i < teamHeros.Count; ++i)
            {
                teamHeros[i].GetComponent<Hero>().BeginTurn();
            }

            Map.instance.DisableAllTiles();
            _isTurnEnded = false;
        }

        protected bool NeedPassHeroTurn()
        {
            return _passHeroTurn || curHero == null || curHero.isTurnEnded;
        }

        // update turn
        public IEnumerator CalcTurn()
        {
            yield return null;

            // it's my turn
            while (!isTurnEnded)
            {
                _passHeroTurn = false;

                curHero = teamHeros[curHeroIndex].GetComponent<Hero>();

                if (curHero != null)
                {
                    GameManager.instance.cameraInput.SetPosition(curHero.transform.position);
                }

                // hero turn
                while ( !NeedPassHeroTurn() )
                {
                    Map.instance.SetCurrentHeroGuide(true, curHero.transform.position);
                    UIManager.instance.SetSelection(UIWindow.Status_Hero, curHero.gameObject);
                    Map.instance.EnableRangeTiles(curHero.position, curHero.moveRange);

                    yield return null;

                    // Do Command
                    while(!NeedPassHeroTurn())
                    {
                        if (isAuto)
                        {
                            yield return new WaitForSeconds(0.5f);
                            // DO Auto
                            curHero.EndTurn();
                            break;
                        }
                        else
                        {
                            // process cmd
                            if (selectedObj != null)
                            {
                                lockSelect = true;

                                IEnumerator e = WaitSelectedObjCommand();

                                while (e.MoveNext() && !NeedPassHeroTurn() )
                                {
                                    yield return e.Current;
                                }

                                yield return null;

                                Map.instance.HideDirGuide();
                                lockSelect = false;
                                break;
                            }
                        }

                        yield return null;
                    }
                }

                UIManager.instance.CloseWindow(UIWindow.Command);
                SetSelectedObject(null);
                ++curHeroIndex;
                if (curHeroIndex < 0 || curHeroIndex >= teamHeros.Count)
                {
                    curHeroIndex = 0;
                }
            }
            Map.instance.SetCurrentHeroGuide(false, Vector3.zero );

            yield return null;
        }

        bool isTurnEnded
        {
            get
            {
                if (_isTurnEnded || teamHeros.Count <= 0)
                    return true;

                for( int i=0;i<teamHeros.Count;++i)
                {
                    Hero curHero = teamHeros[i].GetComponent<Hero>();

                    if (!curHero.isTurnEnded)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        // end turn
        public void EndTurn()
        {
            for (int i = 0; i < teamHeros.Count; ++i)
            {
                teamHeros[i].GetComponent<Hero>().EndTurn();
            }

            Map.instance.EnableAllTiles();
            curHero = null;
        }

        // Re Use
        public override void OnPoolReuse()
        {
            GameManager.instance.AddController(this);

            base.OnPoolReuse();
        }

        // Release
        public override void OnPoolReleased()
        {
            GameManager.instance.RemoveController(this);

            base.OnPoolReleased();
        }

        // process input
        public bool ProcessInput()
        {
            // if down
            if (JInputManager.ButtonDown(0))
            {
                Vector2 wp = Camera.main.ScreenToWorldPoint(JInputManager.GetScreenPosition(0));
                Ray2D ray = new Ray2D(wp, Vector2.zero);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                btDownObj = null;
                if (hit.collider != null)
                {
                    btDownObj = hit.collider.gameObject;
                }

                return true;
            }
            // if up
            else if( btDownObj != null && JInputManager.ButtonUp(0) )
            {
                Vector2 wp = Camera.main.ScreenToWorldPoint(JInputManager.GetScreenPosition(0));
                Ray2D ray = new Ray2D(wp, Vector2.zero);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                
                if (hit.collider != null && btDownObj == hit.collider.gameObject )
                {
                    SetSelectedObject(btDownObj);
                }

                return true;
            }

            return false;
        }

        // add hero.
        public void AddHero(GameObject hero)
        {
            teamHeros.Add(hero);
        }

        // create arlam object.
        public void CreateArlamObj()
        {
            // create selected tile arlram 
            if (arlramObj == null)
            {
                arlramObj = ObjectPoolManager.instance.Pop(Resources.Load<GameObject>("Prefab/Map/Arrow"));

                if (arlramObj != null)
                {
                    arlramObj.SetActive(false);
                    arlramObj.transform.SetParent(transform);
                }
            }
        }

        // set selected object. 
        public void SetSelectedObject( GameObject obj )
        {
            if ( selectedObj == obj || (lockSelect && obj != null) ) return;

            selectedObj = obj;

            UIManager.instance.SetSelection(UIWindow.Status_Selected, selectedObj);

            if ( selectedObj != null && arlramObj != null)
            {
                arlramObj.SetActive(true);

                arlramObj.transform.position = selectedObj.transform.position;
            }
            else if( arlramObj != null )
            {
                arlramObj.SetActive(false);
            }

        }

        // select move command. wait for select
        protected IEnumerator SelectMoveCmd()
        {
            var tile = selectedObj.GetComponent<Tile>();

            object result = null;

            IEnumerator e = UIManager.instance.ShowWindow(UIWindow.Command, (int)UIWnd_Cmd.Move);

            while (e.MoveNext())
            {
                result = e.Current;

                yield return e.Current;
            }

            switch ((UICmd_Move)result)
            {
                case UICmd_Move.Move:
                    curHero.MoveTo(tile);
                    break;
                case UICmd_Move.Rotate:
                    e = RotateCmd();
                    while (e.MoveNext())
                    {
                        yield return e.Current;
                    }
                    break;
                case UICmd_Move.Cancel:
                    break;
            }
        }

        // select attack command. wait for select
        protected IEnumerator SelectAttackCmd()
        {
            object result = null;

            IEnumerator e = UIManager.instance.ShowWindow(UIWindow.Command, (int)UIWnd_Cmd.Attack);

            while (e.MoveNext())
            {
                result = e.Current;

                yield return e.Current;
            }

            switch ((UICmd_Attack)result)
            {
                case UICmd_Attack.Attack:
                    break;
                case UICmd_Attack.Throw:
                    break;
                case UICmd_Attack.Cancel:
                    break;
            }
        }

        protected IEnumerator WaitSelectedObjCommand( )
        {
            var tile = selectedObj.GetComponent<Tile>();

            if (tile != null && tile.isSelecteable)
            {
                IEnumerator e = null;

                Hero other = null;

                if (tile.actor != null)
                {
                    other = tile.actor.GetComponent<Hero>();
                }

                // set command type.
                if( other != null && other != curHero )
                {
                    // attack command
                    e = SelectAttackCmd();
                }
                else
                {
                    // move command
                    e = SelectMoveCmd();                    
                }

                if( e != null )
                {
                    while (e.MoveNext())
                    {
                        yield return e.Current;
                    }
                }

                SetSelectedObject(null);
            }
            yield return null;
        }

        protected IEnumerator RotateCmd()
        {
            SetSelectedObject(null);
            lockSelect = false;
            Map.instance.EnableRangeTiles(curHero.position, 1);
            Map.instance.ShowDirGuide(curHero.position);
            yield return null;

            while (true)
            {
                // if choose dir tile
                if ( selectedObj != null )
                {
                    var tile = selectedObj.GetComponent<Tile>();

                    if (tile != null && tile.isSelecteable)
                    {
                        curHero.LookAt(tile.position);
                        break;
                    }
                }                                
                yield return null;
            }

            yield return null;
        }

        // can attack selected hero?
        public bool CanAttackSelection()
        {
            if (curHero == null || selectedObj == null) return false;

            var tile = selectedObj.GetComponent<Tile>();

            if (tile != null )
            {
                Hero other = null;

                if (tile.actor != null)
                {
                    other = tile.actor.GetComponent<Hero>();
                }

                if (other != null)
                {
                    return curHero.CanAttack(other);
                }
            }

            return false;
        }

        // can throw attack selected hero?
        public bool CanThrowSelection()
        {
            if (curHero == null || selectedObj == null) return false;

            return false;
        }

        // can move to selected tile?
        public bool CanMoveSelection()
        {
            if (curHero == null || selectedObj == null) return false;

            var tile = selectedObj.GetComponent<Tile>();

            if (tile != null && tile.isSelecteable)
            {
                // check can move to
                if ( curHero.CanMoveTo( tile ) )
                {
                    return true;
                }
            }

            return false;
        }

        public void PassCurrentHeroTurn()
        {
            if (curHero != null)
            {
                _passHeroTurn = true;
            }
        }

        public void EndCurrentHeroTurn()
        {
            if (curHero != null)
            {
                curHero.EndTurn();
            }
        }
    }
}
