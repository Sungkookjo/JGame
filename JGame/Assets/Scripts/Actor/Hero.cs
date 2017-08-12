using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JGame.Data;

namespace JGame
{
    [System.Serializable]
    public class HeroInfo
    {
        public string name;
        public int[] soldiers = new int[Config.MaxSoldierNum];
    }

    public class Hero : Unit
    {
        public IntRect position = new IntRect();

        Controller owner = null;

        // hero troops
        protected Squad squad = new Squad();

        private void Awake()
        {
            squad.owner = this;
        }

        // set position
        public void SetPosition(IntRect pos)
        {
            SetPosition(pos.x, pos.y);
        }

        public void SetPosition(int x, int y)
        {
            Map.instance.LeaveFrom(position, gameObject);
            position.x = x;
            position.y = y;
            Map.instance.MoveTo(position, gameObject);

            squad.MoveTo(x, y);
        }

        public void InitializeFromInfo(HeroInfo info)
        {
            if (info == null) return;

            // set name
            name = info.name;

            // add soldiers
            for( int i = 0; i<info.soldiers.Length; ++i)
            {
                squad.SetMembers(i, DataController.CreateSoliderById(info.soldiers[i]) );
            }

            squad.MoveTo(position);
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

        // Re Use
        public override void OnPoolReuse()
        {
            base.OnPoolReuse();
            squad.OnPoolReuse();
        }

        // Release
        public override void OnPoolReleased()
        {
            base.OnPoolReleased();
            squad.OnPoolReleased();
        }
    }
}
