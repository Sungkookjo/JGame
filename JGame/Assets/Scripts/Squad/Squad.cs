using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    public class Squad
    {
        protected Team team = null;
        protected List<GameObject> members = new List<GameObject>();

        public void SetTeam( Team newTeam )
        {
            if (team == newTeam) return;

            team = newTeam;

            for( int i =0; i < members.Count;++i)
            {
                var unit = members[i].GetComponent<Unit>();

                if (unit != null)
                {
                    unit.SetTeam(team);
                }
            }
        }
    }
}
