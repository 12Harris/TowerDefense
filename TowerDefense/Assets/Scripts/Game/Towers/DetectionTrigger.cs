using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class DetectionTrigger : MonoBehaviour
    {

        private SphereCollider _detectionVolume;
        public SphereCollider DetectionVolume => _detectionVolume;

        public event Action<Enemy> _onEnemyDetected;

        public event Action _onFirstEnemyLost;

        public float Radius {get => _detectionVolume.radius; set => _detectionVolume.radius = value;}

        public Vector3 Center {get => _detectionVolume.center; set => _detectionVolume.center = value;}

        private void Awake()
        {
            _detectionVolume = GetComponent<SphereCollider>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                var enemy = other.gameObject.GetComponent<Enemy>();

                _onEnemyDetected?.Invoke(enemy);

                //_targets.Append(enemy);

            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                _onFirstEnemyLost?.Invoke();
                //_targets.RemoveHead();
            }
        }
    }
}