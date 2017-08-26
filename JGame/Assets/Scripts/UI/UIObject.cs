using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    abstract public class UIObject : MonoBehaviour
    {
        public bool resizeByResolution;

        private void Awake()
        {
            InitFromAwake();
        }

        // Use this for initialization
        void Start()
        {
            InitFromStart();
            UIManager.instance.AddUIObject(this);
        }

        protected virtual void InitFromAwake()
        {
            
        }

        protected virtual void InitFromStart()
        {
            Resize();
        }

        // instantiated
        public virtual void OnPoolInstiate()
        {
        }

        // Re Use
        public virtual void OnPoolReuse()
        {
            UIManager.instance.AddUIObject(this);
        }

        // Release
        public virtual void OnPoolReleased()
        {
            UIManager.instance.RemoveUIObject(this);
        }

        public abstract void Resize();
    }

}