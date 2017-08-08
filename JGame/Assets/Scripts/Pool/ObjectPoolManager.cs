﻿using System.Collections.Generic;
using UnityEngine;

namespace JGame.Pool
{
    public class ObjectPoolManager : MonoBehaviour
    {
        // instance
        private static ObjectPoolManager _instance = null;
        public static ObjectPoolManager instance {
            get
            {
                if (_instance == null) CreateInstance();

                return _instance;
            }
        }

        private Dictionary<int, Queue<GameObject>> pool = new Dictionary<int, Queue<GameObject>>();
        
        // Create Instance
        protected static void CreateInstance()
        {
            if (instance != null) return;

            GameObject obj = new GameObject();

            if (obj != null)
            {
                obj.name = "Object Pool";
                _instance = obj.AddComponent<ObjectPoolManager>();
                DontDestroyOnLoad(obj);
            }
        }

        // get object from object pool
        protected GameObject Pop(GameObject keyObject)
        {
            int key = keyObject.GetInstanceID();
            GameObject obj = null;

            // has key??
            if (!pool.ContainsKey(key))
            {
                pool.Add(key, new Queue<GameObject>());
            }

            // if has any obj, get it!
            if (pool[key].Count > 0)
            {
                obj = pool[key].Dequeue();
            }

            // if don't have any pool object.
            // instantiate new object
            if (obj == null)
            {
                obj = Instantiate(keyObject);

                if (obj != null)
                {
                    // add pool object
                    PoolObject po = obj.AddComponent<PoolObject>();
                    if (po != null)
                    {
                        po.keyObj = keyObject;
                        po.OnInstantiate();
                    }

                    obj.transform.SetParent(gameObject.transform);
                }
            }

            // call reuse
            if (obj != null)
            {
                PoolObject po = obj.GetComponent<PoolObject>();
                if (po != null)
                {
                    po.OnReuse();
                }
            }
            return obj;
        }

        // push object to pool
        protected void Push(GameObject obj)
        {
            PoolObject po = obj.GetComponent<PoolObject>();

            // if is not pool object. destroy
            if (po == null || po.keyObj == null)
            {
                Destroy(obj);
                return;
            }

            int key = po.keyObj.GetInstanceID();

            // has key?
            if (!pool.ContainsKey(key))
            {
                pool.Add(key, new Queue<GameObject>());
            }

            // release
            if (po != null)
            {
                po.OnRelease();
            }

            // SetParent( this )
            if (obj.transform.parent == null)
            {
                obj.transform.SetParent(gameObject.transform);
            }

            // push
            pool[key].Enqueue(obj);
        }
    }
}