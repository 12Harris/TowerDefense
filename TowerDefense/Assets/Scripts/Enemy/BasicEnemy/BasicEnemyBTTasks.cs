using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

namespace TowerDefense
{
    public class BasicEnemyBTTasks : EnemyBTTasks
    {

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        [Task]
		public override void Move()
		{
            _enemy.MoveAlongWaypointPath();
		}

    }
}
