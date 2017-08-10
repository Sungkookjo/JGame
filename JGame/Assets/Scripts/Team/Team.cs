using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    public class Team
    {
        //protected List<Team> alianceList;

        public int teamIndex;

        public bool IsSameTeam(Team other)
        {
            return (this == other || teamIndex == other.teamIndex);
        }
    }
}
