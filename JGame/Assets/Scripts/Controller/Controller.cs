using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JGame.Pool;
using System.Reflection;
using System.Security.Permissions;

namespace JGame
{
    public class Controller : Actor
    {
        public enum State
        {
            BeginTurn,
            WaitCmd,
            Move,
            Rotate,
            Attack,
            Throw,
            Battle,
            EndTurn,
        }

        // is auto?
        public bool isAuto = false;
        
        // cached object last selected
        GameObject selectedObj = null;
        // selected tile arlram
        GameObject arlramObj = null;

        bool lockSelect = false;

        Hero curHero = null;
        int curHeroIndex;
        protected bool _isTurnEnded;

        // heros
        protected List<GameObject> teamHeros = new List<GameObject>();
        
        protected Controller.State _state = State.EndTurn;
        protected IEnumerator coroutine_Update = null;

        public Controller.State state { get { return _state; } }
        
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
        
        bool IsTurnEnded
        {
            get
            {
                if (_isTurnEnded || teamHeros.Count <= 0)
                    return true;

                foreach (var hero in teamHeros)
                {
                    if (hero != null && hero.GetComponent<Hero>() != null)
                    {
                        if (hero.GetComponent<Hero>().isTurnEnded == false)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        // begin turn
        public void BeginTurn()
        {
            SetSelectedObject(null);

            foreach( var hero in teamHeros )
            {
                if( hero != null && hero.GetComponent<Hero>() != null )
                {
                    hero.GetComponent<Hero>().BeginTurn();
                }
            }

            _isTurnEnded = false;
            curHeroIndex = GetBestHeroIndex(0);

            SetState(State.BeginTurn, true);
        }

        // end turn
        public void EndTurn()
        {
            foreach (var hero in teamHeros)
            {
                if (hero != null && hero.GetComponent<Hero>() != null)
                {
                    hero.GetComponent<Hero>().EndTurn();
                }
            }
            
            curHero = null;

            SetState(State.EndTurn, true);
        }

        #region State
        public void SetState(Controller.State newState , bool bForce = false )
        {
            if( _state == newState && !bForce )
            {
                return;
            }

            var oldState = _state;
            DoEndState(newState);
            _state = newState;
            DoBeginState(oldState);
            coroutine_Update = GetStateFunc();
        }

        protected void DoBeginState(Controller.State PrevState)
        {
#if UNITY_EDITOR
            Debug.Log("Begin state - " + name + " '" + PrevState.ToString() + "' -> '"+ _state.ToString() + "'");
#endif
            switch ( _state )
            {
                case State.BeginTurn:
                    State_BeginTurn_Begin(PrevState);
                    break;
                case State.WaitCmd:
                    State_WaitCmd_Begin(PrevState);
                    break;
                case State.Move:
                    State_Move_Begin(PrevState);
                    break;
                case State.Rotate:
                    State_Rotate_Begin(PrevState);
                    break;
                case State.EndTurn:
                    State_EndTurn_Begin(PrevState);
                    break;
            }
        }

        protected IEnumerator GetStateFunc()
        {
            switch( _state )
            {
                case State.BeginTurn:
                    yield return State_BeginTurn();
                    break;
                case State.WaitCmd:
                    yield return State_WaitCmd();
                    break;
                case State.Move:
                    yield return State_Move();
                    break;
                case State.Rotate:
                    yield return State_Rotate();
                    break;
                case State.EndTurn:
                    yield return State_EndTurn();
                    break;
            }

            yield return null;
        }

        protected void DoEndState(Controller.State NextState)
        {
            switch (_state)
            {
                case State.BeginTurn:
                    State_BeginTurn_End(NextState);
                    break;
                case State.WaitCmd:
                    State_WaitCmd_End(NextState);
                    break;
                case State.Move:
                    State_Move_End(NextState);
                    break;
                case State.Rotate:
                    State_Rotate_End(NextState);
                    break;
                case State.EndTurn:
                    State_EndTurn_End(NextState);
                    break;
            }
        }
        #endregion

        #region State func
        #region State_BeginTurn
        protected void State_BeginTurn_Begin(Controller.State PrevState)
        {
        }
        protected IEnumerator State_BeginTurn()
        {
            yield return new WaitForSeconds(0.5f);

            SetState(State.WaitCmd);
        }
        protected void State_BeginTurn_End(Controller.State NextState)
        {

        }
        #endregion

        #region State_WaitCmd
        protected void State_WaitCmd_Begin(Controller.State PrevState)
        {
            // release selected object
            SetSelectedObject(null);

            curHero = teamHeros[curHeroIndex].GetComponent<Hero>();

            // move camera to current hero position
            GameManager.instance.cameraInput.SetPosition(curHero.transform.position);

            // draw hero point
            Map.instance.SetCurrentHeroGuide(true, curHero.transform.position);

            // update hero status ui
            UIManager.instance.SetSelection(UIWindow.Status_Hero, curHero.gameObject);

            // enable moveable tile
            Map.instance.EnableRangeTiles(curHero.position, curHero.moveRange);
        }
        protected IEnumerator State_WaitCmd()
        {
            while ( state == State.WaitCmd )
            {
                // DO Auto
                if (isAuto)
                {
                    //{{@Test
                    yield return new WaitForSeconds(0.5f);
                    EndCurrentHeroTurn();
                    break;
                    //}}@Test
                }
                else
                {
                    // process cmd
                    if (selectedObj != null)
                    {
                        var tile = selectedObj.GetComponent<Tile>();

                        // if selected tile
                        if (tile != null )
                        {
                            Hero other = null;

                            if (tile.actor != null)
                            {
                                other = tile.actor.GetComponent<Hero>();
                            }

                            // set command type.
                            if (other != null && other != curHero)
                            {
                                // attack command
                                UIManager.instance.ShowWindow(UIWindow.Command, (int)UIWnd_Cmd.Attack);
                            }
                            else if( tile.isSelecteable )
                            {
                                // move command
                                UIManager.instance.ShowWindow(UIWindow.Command, (int)UIWnd_Cmd.Move);
                            }
                            else
                            {
                                UIManager.instance.CloseWindow(UIWindow.Command);
                            }
                        }
                    }
                }

                yield return null;
            }
        }
        protected void State_WaitCmd_End(Controller.State NextState)
        {
            // update hero status ui
            UIManager.instance.SetSelection(UIWindow.Status_Hero, null);

            // enable all tile
            Map.instance.EnableAllTiles();

            UIManager.instance.CloseWindow(UIWindow.Command);
        }
        #endregion

        #region State_Move
        protected void State_Move_Begin(Controller.State PrevState)
        {
            curHero = teamHeros[curHeroIndex].GetComponent<Hero>();
        }
        protected IEnumerator State_Move()
        {
            yield return null;

            if (curHero != null && selectedObj != null)
            {
                var tile = selectedObj.GetComponent<Tile>();

                if (curHero.CanMoveTo(tile))
                {
                    curHero.MoveTo(tile);
                }
            }

            yield return new WaitForSeconds( 0.1f );
        }
        protected void State_Move_End(Controller.State NextState)
        {
            SetSelectedObject(null);
        }
        #endregion

        #region State_Rotate
        protected void State_Rotate_Begin(Controller.State PrevState)
        {
            // release selected object
            SetSelectedObject(null);

            // enable only 1x1 tiles
            Map.instance.EnableRangeTiles(curHero.position, 1);

            // show direction guide ui
            Map.instance.ShowDirGuide(curHero.position);
        }
        protected IEnumerator State_Rotate()
        {
            yield return null;

            while ( state == Controller.State.Rotate )
            {
                // if choose dir tile
                if (selectedObj != null)
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

            yield return new WaitForSeconds(0.1f);
        }
        protected void State_Rotate_End(Controller.State NextState)
        {
            // enable all tile
            Map.instance.EnableAllTiles();
            Map.instance.HideDirGuide();

            SetSelectedObject(null);
        }
        #endregion

        #region State_EndTurn
        protected void State_EndTurn_Begin(Controller.State PrevState)
        {
            _isTurnEnded = true;
        }

        protected IEnumerator State_EndTurn()
        {
            yield return null;
        }
        protected void State_EndTurn_End(Controller.State NextState)
        {

        }
        #endregion

        #endregion

        // update turn
        public IEnumerator CalcTurn()
        {
            yield return null;

            while (!IsTurnEnded)
            {
                if (coroutine_Update == null)
                {
                    SetState(State.WaitCmd);
                }

                if (coroutine_Update != null)
                {
                    IEnumerator e = coroutine_Update;
                    if (e.MoveNext())
                    {
                        yield return e.Current;
                    }
                    else
                    {
                        coroutine_Update = null;
                        yield return null;
                    }
                }
                else
                {
#if UNITY_EDITOR
                    Debug.LogError(" can't find update func.");
#endif
                    yield return null;
                }
            }

            yield return null;
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
            // if up
            if( JInputManager.ButtonUp(0) )
            {
                Vector2 wp = Camera.main.ScreenToWorldPoint(JInputManager.GetScreenPosition(0));
                Ray2D ray = new Ray2D(wp, Vector2.zero);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                
                if (hit.collider != null )
                {
                    SetSelectedObject(hit.collider.gameObject);
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

        // pass turn
        public void PassCurrentHeroTurn()
        {
            if (curHero != null)
            {
                curHeroIndex = GetBestHeroIndex(curHeroIndex+1);
                
                if (curHeroIndex == -1)
                {
                    SetState(State.EndTurn);
                }
                else
                {
                    SetState(State.WaitCmd, true);
                }
            }
        }

        // end turn
        public void EndCurrentHeroTurn()
        {
            if (curHero != null)
            {
                curHero.EndTurn();
                curHeroIndex = GetBestHeroIndex( curHeroIndex+1);

                if( curHeroIndex == -1 )
                {
                    SetState(State.EndTurn);
                }
                
                if( !IsTurnEnded )
                {
                    SetState(State.WaitCmd, true);
                }
            }
        }

        // get best hero index
        public int GetBestHeroIndex(int idx)
        {
            if (idx < 0 || idx >= teamHeros.Count) idx = 0;

            for(int i = idx; i < teamHeros.Count; ++i )
            {
                if(teamHeros[i] != null && teamHeros[i].GetComponent<Hero>() != null )
                {
                    if (!teamHeros[i].GetComponent<Hero>().isTurnEnded && !teamHeros[i].GetComponent<Hero>().isDead)
                        return i;
                }
            }

            for( int i = 0; i <= idx;++i)
            {
                if (teamHeros[i] != null && teamHeros[i].GetComponent<Hero>() != null)
                {
                    if (!teamHeros[i].GetComponent<Hero>().isTurnEnded && !teamHeros[i].GetComponent<Hero>().isDead)
                        return i;
                }
            }

            return -1;
        }
    }
}
