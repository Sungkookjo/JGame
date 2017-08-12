using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    public class Team
    {
        //protected List<Team> alianceList;

        public int teamIndex = Config.teamNone;

        public bool IsSameTeam(Team other)
        {
            if (teamIndex == Config.teamNone) return false;

            return (this == other || teamIndex == other.teamIndex);
        }
    }
}
