using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    public class Soldier : Unit
    {
        public bool hasDirAnim;
        protected IntRect rotation = new IntRect();

        public Sprite iconImg;

        public byte level;

        public bool IsAlivedAndWell()
        {
            return true;
        }

        #region Rotate
        public void SetRotation(IntRect r)
        {
            SetRotation(r.x, r.y);
        }

        public void SetRotation(int x, int y)
        {
            rotation.x = x;
            rotation.y = y;
            
            if ( !hasDirAnim )
            {
                Vector3 angle = new Vector3(0, 0, 15);
                Vector3 scale = new Vector3(1, 1, 1);

                if( y == 0 )
                {
                    angle *= -1.0f;
                }

                if (x < 0 || y > 0 )
                {
                    scale.x *= -1.0f;
                }

                transform.rotation = Quaternion.Euler(angle);
                transform.localScale = scale;
            }
        }
        #endregion
    }
}
