using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JGame.Pool;

namespace JGame
{
    public class Squad
    {
        protected IntRect position = new IntRect();
        protected IntRect rotation = new IntRect();

        public Hero owner;
        public Team team = null;
        protected Soldier[] members = new Soldier[Config.MaxSoldierNum];

        // Set member
        public void SetMembers( int idx , Soldier newMember )
        {
            if (idx < 0 || idx >= members.Length) return;
            
            members[idx] = newMember;

            if(members[idx] != null)
            {
                members[idx].SetTeam(team);
                members[idx].transform.SetParent(owner.transform);
                members[idx].transform.position = GetMemberPosition( idx );
                members[idx].SetRotation(rotation);
            }
        }

        public void SetSelectable(bool selectable)
        {
            foreach (var member in members)
            {
                if( member != null )
                {
                    member.SetSelectable(selectable);
                }
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
            var DestPos = position;

            position.x = x;
            position.y = y;

            Rotate(position - DestPos);

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
        #region rotate
        public void Rotate( IntRect r)
        {
            Rotate(r.x, r.y);
        }
        public void Rotate(int x, int y)
        {
            rotation.x = x;
            rotation.y = y;

            if (x == 0)
            {
                rotation.x = -y;
            }
            if (y == 0)
            {
                rotation.y = x;
            }

            rotation.Normalize();

            for (int i = 0; i < members.Length; ++i)
            {
                if (members[i] != null)
                {
                    members[i].SetRotation(rotation);
                }
            }
        }
        #endregion

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

        public int GetMoveRange()
        {
            return 2;
        }
    }
}
