using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JGame.Pool;

namespace JGame
{
    public class Squad
    {
        protected IntRect position = new IntRect();
        public Hero owner;
        public Team team = null;
        protected Soldier[] members = new Soldier[Config.MaxSoldierNum];

        // Set member
        public void SetMembers( int idx , Soldier newMember )
        {
            if (idx < 0 || idx >= members.Length) return;
            
            members[idx] = newMember;

            if(newMember != null)
            {
                newMember.SetTeam(team);
                newMember.transform.SetParent(owner.transform);
                newMember.transform.position = GetMemberPosition( idx );
            }
        }

        // set team
        public void SetTeam( Team newTeam )
        {
            if (team == newTeam) return;

            team = newTeam;

            // change members team
            for( int i =0; i < members.Length;++i)
            {
                var unit = members[i].GetComponent<Unit>();

                if (unit != null)
                {
                    unit.SetTeam(team);
                }
            }
        }

        public void MoveTo( IntRect newPos )
        {
            MoveTo(newPos.x, newPos.y);
        }

        public Vector3 GetMemberPosition( int i )
        {
            return GetMemberPosition(i% 3, i / 3);
        }

        public Vector3 GetMemberPosition(int x, int y)
        {
            var pos = Map.instance.GetTile(position).transform.position;
            pos += Map.instance.tileSpacingX * 0.3f * (x - 1);
            pos += Map.instance.tileSpacingY * 0.3f * (y - 1);

            return pos;
        }

        public void MoveTo(int x, int y)
        {
            position.x = x;
            position.y = y;

            // set members position
            for (int i = 0; i < members.Length; ++i)
            {
                // {{ @Test
                if (members[i] != null)
                {
                    members[i].transform.position = GetMemberPosition(i);
                }
                // }} @Test
            }
        }

        // reuse
        public void OnPoolReuse()
        {

        }

        // released
        public void OnPoolReleased()
        {
            for (int i = 0; i < members.Length; ++i)
            {
                if (members[i] != null)
                {
                    ObjectPoolManager.instance.Push(members[i].gameObject);
                }
                members[i] = null;
            }
        }
    }
}
