using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    public class Controller : Actor
    {
        // heros
        protected List<GameObject> teamHeros = new List<GameObject>();

        // is auto?
        public bool isAuto = false;

        private void Awake()
        {
            GameManager.instance.allController.Add(this);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

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
    }
}
