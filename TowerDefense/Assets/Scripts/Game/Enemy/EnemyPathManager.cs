using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.GPC;
using UnityEditor;

namespace TowerDefense
{
    public class EnemyPathManager : MonoBehaviour
    {
        //all paths in the scene
        private static List<BezierPathController> _enemyPaths = new List<BezierPathController>();
        public static int PathCount => _enemyPaths.Count;
        public static Vector3 EnemyStart => GameObject.Find("EnemyStart").transform.position;
       
        private void Awake()
        {  
            WaypointsController._onCreated += AddPath;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        private static void AddPath(WaypointsController path)
        {
            _enemyPaths.Add(path as BezierPathController);
        }

        public static BezierPathController GetNearestPath(Vector3 location)
        {
            float minDistance = 1000;
            BezierPathController nearestPath = null;
            foreach(BezierPathController path in _enemyPaths)
            {
                var dist = Vector3.Distance(path.firstPoint, location);
                if(dist < minDistance)
                {
                    minDistance = dist;
                    nearestPath = path;

                }
            }
            return nearestPath;
        }

        public static BezierPathController GetNearestPath(Enemy enemy)
        {
            return GetNearestPath(enemy.transform.position);
        }

    }
}
