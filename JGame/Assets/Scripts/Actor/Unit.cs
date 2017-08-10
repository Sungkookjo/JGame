using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JGame
{
    public class Unit : Actor
    {
        protected bool _isDead;
        public bool isDead { get { return _isDead; } }
        protected int health = 0;
        protected int attack = 0;
        protected int defence = 0;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
