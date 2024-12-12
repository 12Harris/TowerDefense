using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.GPC;

namespace TowerDefense
{
    public abstract class Enemy : MonoBehaviour
    {
        private AIBotController _botController;
        private Health _health;

        protected virtual void Awake()
        {
            _botController = GetComponent<AIBotController>();
        }
        // Start is called before the first frame update
        protected virtual void Start()
        {
           
        }

        // Update is called once per frame
        protected virtual void Update()
        {
        
        }

        public void MoveAlongWaypointPath()
        {
            _botController.MoveAlongWaypointPath();
        }
    }
}
