using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TowerDefense
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private int _maxHealth;
        public int MaxHealth => _maxHealth;

        private int _currentHealth;

        public int CurrentHealth => _currentHealth;

        //[SerializeField]
        private HealthBar _healthBar;

        [SerializeField]
        [Range(2, 10)]
        private float _drainSpeed = 5f;

        public event Action _onReachedTargetHealth;

        private bool _noHealth = false;

        public event Action _onLostAllHealth;

        [SerializeField]
        private int _type = 0;

        [SerializeField]
        private Transform _healthBarPosition;

        public void Start()
        {
            _currentHealth = _maxHealth;
            
            if(_type == 1)
                _healthBar = UIManager.SpawnHealthBar1(_healthBarPosition);
            else if(_type == 2)
                _healthBar = UIManager.SpawnHealthBar2(_healthBarPosition);
            _healthBar.Reset();
        }

        public void DrainHealth(int amount)
        {
            if (_currentHealth <= 0f)
                return;

            var targetHealth = _currentHealth - amount;

            if (targetHealth <= 0)
                targetHealth = 0;

            DrainTowards(targetHealth);

        }

        public void DrainTowards(int targetHealth)
        {
            StartCoroutine(_DrainTowards(targetHealth));
        }

        private IEnumerator _DrainTowards(int targetHealth)
        {
            while (_currentHealth > targetHealth)
            {
                _currentHealth--;
                var v = (float)(_currentHealth) / 100f;
                _healthBar.CurrentFillAmount = (float)(_currentHealth) / 100f;
                yield return new WaitForSeconds(0.1f);
            }

            Debug.Log("Current Health = " + _currentHealth);

            //check whether the bar is empty
            if (_currentHealth <= 0f)
            {
                if (_noHealth == false)
                {
                    _onLostAllHealth?.Invoke();
                    _noHealth = true;
                }
            }

            _onReachedTargetHealth?.Invoke();
        }
    }
}