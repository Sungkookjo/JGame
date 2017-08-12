using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    public class Actor : MonoBehaviour
    {
        [HideInInspector]
        public Team team = null;        

        // SetTeam
        public virtual void SetTeam( Team newTeam )
        {
            if( team == newTeam )
            {
                return;
            }

            team = newTeam;
        }

        // instantiated
        public virtual void OnPoolInstiate()
        {
        }

        // Re Use
        public virtual void OnPoolReuse()
        {
        }

        // Release
        public virtual void OnPoolReleased()
        {
        }
    }
}
