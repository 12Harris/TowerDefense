using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;

namespace TowerDefense
{
    public abstract class EnemyBTTasks : MonoBehaviour
    {
        protected Enemy _enemy;
        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

         [Task]
		public virtual void Move()
		{
            if(!_enemy.reachedLastWaypoint)
                _enemy.Move();
            else
            {
                _enemy.Stop();
                ThisTask.Succeed();
            }
		}

        [Task]
        public void MoveToTargetPlant()
        {
            _enemy.MoveToTargetplant();
        }

        [Task]
        public virtual void Destroy()
        {
            _enemy.Destroy();
            ThisTask.Succeed();
        }

    }
}
