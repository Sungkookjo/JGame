using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame.Pool
{
    public class PoolObject_Actor : PoolObject
    {
        protected Actor CachedActor = null;

        // instantiated
        public override void OnInstantiate()
        {
            base.OnInstantiate();

            CachedActor = gameObject.GetComponent<Actor>();

            if( CachedActor != null )
            {
                CachedActor.OnPoolInstiate();
            }
        }

        // Re Use
        public override void OnReuse()
        {
            base.OnReuse();

            if (CachedActor != null)
            {
                CachedActor.OnPoolReuse();
            }
        }

        // Release
        public override void OnRelease()
        {
            base.OnRelease();

            if (CachedActor != null)
            {
                CachedActor.OnPoolReleased();
            }
        }
    }
}
