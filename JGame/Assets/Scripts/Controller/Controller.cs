using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JGame.Pool;

namespace JGame
{
    public class Controller : Actor
    {
        GameObject arlramObj = null;

        // heros
        protected List<GameObject> teamHeros = new List<GameObject>();

        // is auto?
        public bool isAuto = false;

        // change team
        public override void SetTeam( Team newTeam )
        {
            base.SetTeam(newTeam);

            if (team == newTeam) return;

            for( int i = 0; i < teamHeros.Count; ++i )
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
        }

        // update turn
        public IEnumerator CalcTurn()
        {
            if( isAuto )
            {
                // DO Auto
            }
            yield return null;
        }

        // end turn
        public void EndTurn()
        {
        }

        // Re Use
        public override void OnPoolReuse()
        {
            GameManager.instance.allController.Add(this);

            base.OnPoolReuse();
        }

        // Release
        public override void OnPoolReleased()
        {
            GameManager.instance.allController.Remove(this);

            base.OnPoolReleased();
        }

        public bool ProcessInput()
        {
            if (JInputManager.ButtonDown(0))
            {
                Vector2 wp = Camera.main.ScreenToWorldPoint(JInputManager.GetScreenPosition(0));
                Ray2D ray = new Ray2D(wp, Vector2.zero);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null)
                {
                    if(arlramObj != null)
                    {
                        arlramObj.transform.position = hit.collider.transform.position;
                    }
                }

                return true;
            }

            return false;
        }

        public void CreateArlamObj()
        {
            arlramObj = ObjectPoolManager.instance.Pop( Resources.Load<GameObject>("Prefab/Map/Arlram") );
        }
    }
}
