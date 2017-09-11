using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    [System.Serializable]
    public enum UIPanel_AnimType
    {
        None,
        LeftToRight,
        RightToLeft,
        ScaleUp,
        ScaleDown,
    }

    public class UIPanel : UIObject
    {
        #region active/deactive anim
        public UIPanel_AnimType activateAnimType = UIPanel_AnimType.None;
        public float activateAnimTime = 0.1f;
        public UIPanel_AnimType deactivateAnimType = UIPanel_AnimType.None;
        public float deactivateAnimTime = 0.1f;

        protected IEnumerator activeCoroutine = null;
        #endregion

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

        public void SetActive(bool bShow, bool bSkipAnim = false )
        {
            if ( bShow == gameObject.activeInHierarchy)
            {
                return;
            }

            if (bSkipAnim)
            {
                gameObject.SetActive(bShow);
            }
            else
            {
                // set active
                gameObject.SetActive(true);

                // if doing. Stop
                if(activeCoroutine != null )
                {
                    StopCoroutine(activeCoroutine);
                    activeCoroutine = null;
                }

                activeCoroutine = DoAnimAndActive(bShow);
                StartCoroutine(activeCoroutine);
            }
        }

        // play animation and setactive
        protected IEnumerator DoAnimAndActive( bool bShow )
        {
            IEnumerator e = null;

            // do animation
            if (bShow)
            {
                e = DoAnim(activateAnimType, activateAnimTime);
            }
            else
            {
                e = DoAnim(deactivateAnimType, deactivateAnimTime);
            }

            while( e.MoveNext() )
            {
                yield return e.Current;
            }

            // set disable
            if( !bShow )
            {
                gameObject.SetActive(false);
            }
            
            activeCoroutine = null;
        }
        
        protected IEnumerator DoAnim(UIPanel_AnimType animType,float animTime)
        {
            if( animType == UIPanel_AnimType.None || animTime <= 0.0f )
            {
                yield break;
            }

            yield return null;

            var duration = animTime;

            switch(animType)
            {
                case UIPanel_AnimType.LeftToRight:
                    break;
                case UIPanel_AnimType.RightToLeft:
                    break;
                case UIPanel_AnimType.ScaleDown:
                    {
                        var scale = transform.localScale.x;
                        transform.localScale = new Vector3(scale, scale, scale);
                        yield return null;
                        while (duration > 0.0f)
                        {
                            duration -= Time.deltaTime;
                            scale = Mathf.Max(0.0f, scale - (Time.deltaTime * (1.0f / animTime)));
                            transform.localScale = new Vector3(scale, scale, scale);
                            yield return null;
                        }
                    }
                    break;
                case UIPanel_AnimType.ScaleUp:
                    {
                        var scale = transform.localScale.x;
                        transform.localScale = new Vector3(scale, scale, scale);
                        yield return null;
                        while (duration > 0.0f)
                        {
                            duration -= Time.deltaTime;
                            scale = Mathf.Min(1.0f, scale + (Time.deltaTime * (1.0f / animTime)));
                            transform.localScale = new Vector3(scale, scale, scale);
                            yield return null;
                        }
                    }
                    break;
            }
        }
    }

}