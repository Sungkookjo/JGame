using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    public class Hero : Unit
    {
        Controller owner = null;
        // hero troops
        protected Squad squad = new Squad();
        
        protected int Str;
        protected int Dex;
        protected int Con;
        protected int Int;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        // set owner
        public void SetOwner( Controller newOwner)
        {
            if (owner == newOwner) return;

            owner = newOwner;

            if (owner == null) return;

            SetTeam(owner.team);
            transform.SetParent(newOwner.transform);
        }

        // change team
        public override void SetTeam(Team newTeam)
        {
            base.SetTeam(newTeam);

            if (team == newTeam) return;

            squad.SetTeam(newTeam);
        }
    }
}
