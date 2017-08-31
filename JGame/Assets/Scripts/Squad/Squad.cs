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
        protected Soldier[] formation = new Soldier[Config.MaxSoldierNum];

        // Set member
        public void SetMembers( int idx , Soldier newMember )
        {
            if (idx < 0 || idx >= members.Length) return;
            
            members[idx] = newMember;

            if(members[idx] != null)
            {
                members[idx].SetTeam(team);
                members[idx].transform.SetParent(owner.transform);
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

        public void UpdateMembersPosition()
        {
            for( int i = 0; i < formation.Length;++i )
            {
                if(formation[i] != null)
                {
                    formation[i].transform.position = GetMemberPosition(i);
                }
            }
        }

        public void UpdateSquadFormation()
        {
            int[,] matFormation = null;
            if( rotation.x == -1 )
            {
                if (rotation.y == 1)
                {
                    matFormation = new int[,] { { 8, 7, 6 }, { 5, 4, 3 }, { 2, 1, 0 } };
                }
                else
                {
                    matFormation = new int[,] { { 2, 5, 8 }, { 1, 4, 7 }, { 0, 3, 6 } };
                }
            }
            else
            {
                if (rotation.y == 1)
                {
                    matFormation = new int[,] { { 6, 3, 0 }, { 7, 4, 1 }, { 8, 5, 2 } };
                }
                else
                {
                    matFormation = new int[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } };
                }
            }
            

            for (int y = 0; y < 3; ++y)
            {
                for( int x=0;x<3;++x)
                {
                    formation[y * 3 + x] = members[(int)matFormation[y,x]];
                }
            }
        }

        public void UpdateMembersRotation()
        {
            foreach (var mem in members)
            {
                if( mem != null )
                {
                    mem.SetRotation(rotation);
                }
            }
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

            SetRotation(position - DestPos);

            UpdateMembersPosition();
        }
        #region rotate
        public void SetRotation( IntRect r)
        {
            SetRotation(r.x, r.y);
        }
        public void SetRotation(int x, int y)
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

            UpdateSquadFormation();
            UpdateMembersRotation();
            UpdateMembersPosition();
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

        public void LookAt(IntRect target)
        {
            SetRotation(target - position);
        }
    }
}
