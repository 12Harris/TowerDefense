using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.GPC;
using System;

namespace TowerDefense
{
    public abstract class Enemy : MonoBehaviour
    {
        private AIBotController _botController;
        private static Health _health;
        public static Health Health => _health;
        public bool reachedLastWaypoint = false;
        private WaypointsController current_waypointsController;

        private int currentPathIndex = 0;

        private bool initialized = false;

        public static event Action _onDestroyed;

        protected static float MaxSpeed = 0;

        protected static float Armor = 0;

        protected static float R_MachineGun = 0;

        protected static float R_LaserTurret = 0;

        protected static float R_RocketTurret = 0;

        protected static float R_Cannon = 0;

        private List<Turret> _p_attackers;

        public List<Turret> P_Attackers => _p_attackers;

        protected virtual void Awake()
        {
            _botController = GetComponent<AIBotController>();
            _botController._onReachedLastWaypoint += handleReachedEndOfPath;
            _health = GetComponent<Health>();
            _p_attackers = new List<Turret>();
        }

        private void handleReachedEndOfPath()
        {
            if(currentPathIndex < EnemyPathManager.PathCount-1)
            {
                currentPathIndex++;
                UpdateWaypointController();
            }
            else
            {
                reachedLastWaypoint = true;
            }
        }

        public void AddPotentialAttacker(Turret turret)
        {
            _p_attackers.Add(turret);
        }

        public void RemovePotentialAttacker(Turret turret)
        {
            _p_attackers.Add(turret);
        }


        private void UpdateWaypointController()
        {
            current_waypointsController = EnemyPathManager.GetNearestPath(this);
            current_waypointsController.GetTransforms();
           _botController.SetWayController(current_waypointsController);
           
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            //UpdateWaypointController();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if(!initialized)
            {
                UpdateWaypointController();
                initialized = true;
            }
        }

        public virtual void Move()
        {
            _botController.MoveAlongWaypointPath();
        }

        public virtual void Stop()
        {
            _botController.Stop();
        }

        public void Destroy()
        {
            GameObject.Destroy(gameObject);
            _onDestroyed?.Invoke();
        }
    }
}
