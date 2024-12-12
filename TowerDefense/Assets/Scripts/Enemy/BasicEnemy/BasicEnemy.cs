using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.GPC;

namespace TowerDefense
{
    public class BasicEnemy : Enemy
    {
        private AIBotController _botController;
        private Health _health;

        protected override void Awake()
        {
            base.Awake();
        }
        // Start is called before the first frame update
        protected override void Start()
        {
           base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}
