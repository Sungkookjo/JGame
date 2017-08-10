using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    public class Actor : MonoBehaviour
    {
        protected Team team = null;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

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
