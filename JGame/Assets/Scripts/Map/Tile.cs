using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    public class Tile : MonoBehaviour
    {
        // tile index (x,y)
        public IntRect position;

        // actor on tile
        public GameObject actor;
        public SpriteRenderer spriteRenderer;

        public bool isSelecteable = true;

        static protected Color selectableColor = new Color(1,1,1,1);
        static protected Color notSelectableColor = new Color(0.5f, 0.5f, 0.5f,1.0f);

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            SetSelectable(true);
        }

        public void SetSelectable( bool selectable )
        {
            if (isSelecteable == selectable) return;

            isSelecteable = selectable;

            if (isSelecteable)
            {
                spriteRenderer.color = selectableColor;
            }
            else
            {
                spriteRenderer.color = notSelectableColor;
            }

            if( actor != null )
            {
                Actor act = actor.GetComponent<Actor>();

                if( act != null )
                {
                    act.SetSelectable(isSelecteable);
                }
            }
        }
    }
}
