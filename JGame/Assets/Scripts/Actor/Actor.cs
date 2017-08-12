using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    public class Actor : MonoBehaviour
    {
        [HideInInspector]
        public Team team = null;

        public IntRect position = new IntRect();

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        // set position
        public void SetPosition(IntRect pos)
        {
            Map.instance.LeaveFrom(position, gameObject);
            position.x = pos.x;
            position.y = pos.y;
            Map.instance.MoveTo(position, gameObject);
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
