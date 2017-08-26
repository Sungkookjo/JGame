using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    public class UIPanel : UIObject
    {
        protected RectTransform rectTransform;
        protected Vector2 rectSize;

        protected override void InitFromAwake()
        {
            rectTransform = GetComponent<RectTransform>();

            if( resizeByResolution )
            {
                rectSize = rectTransform.sizeDelta;
            }

            base.InitFromAwake();
        }

        protected override void InitFromStart()
        {
            base.InitFromStart();
        }

        public override void Resize()
        {
            if( resizeByResolution )
            {
                var newSize = rectSize;
                newSize *= JUtil.GetMinResRatio();

                rectTransform.sizeDelta = newSize;
            }
        }
    }

}