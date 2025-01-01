using System.Collections;
using System.Collections.Generic;
using Harris.GPC;
using TowerDefense;
using UnityEngine;

namespace TowerDefense
{
    public class LevelManager : Singleton<LevelManager>
    {
        // Start is called before the first frame update

        [SerializeField]
        private Enemy[] _enemyPrefabs;

        private float _spawnTimer = 0;
        private int _spawnedEnemies = 0;
        private int _currentWave = 1;
        private int _maxEnemiesToSpawn = 0;
        private int _enemiesAlive = 0;

        [SerializeField]
        private float _timeBetweenWaves = 1f;

        private float difficultyFactor = 0.75f;

        private bool _isSpawning = false;

        [SerializeField]
        private List<LevelInfo> _levelInfoList;

        private LevelInfo _currentLevel;
        private IEnumerator StartWave()
        {
            if(_currentWave > 1)
                yield return new WaitForSeconds(_timeBetweenWaves);
            else 
                yield return null;
            _isSpawning = true;
            _maxEnemiesToSpawn = _currentLevel._waves[_currentWave-1]._enemyCount;
        }

        private void EndWave()
        {
            _isSpawning = false;
            _spawnedEnemies = 0;
            _currentWave++;

            if(_currentWave <= _currentLevel._waves.Length)
            StartCoroutine(StartWave());

        }

        private void Awake()
        {
            _currentLevel =_levelInfoList[0];
            Enemy._onDestroyed += handleEnemyDestroyed;
        }

        private void handleEnemyDestroyed()
        {
            _enemiesAlive--;
        }
        

        void Start()
        {
            StartCoroutine(StartWave());
        }
        // Update is called once per frame
        void Update()
        {

            if(!_isSpawning)
                return;

            _spawnTimer += Time.deltaTime;
            if(_spawnTimer >= _currentLevel._waves[_currentWave-1]._spawnInterval && _spawnedEnemies < _maxEnemiesToSpawn)
            {
                Enemy enemy = Instantiate(_enemyPrefabs[_currentLevel._waves[_currentWave-1]._enemyType], EnemyPathManager.EnemyStart,Quaternion.identity);
                _spawnTimer = 0f;
                _spawnedEnemies++;
                _enemiesAlive++;
            }

            //End wave
            if(_spawnedEnemies == _maxEnemiesToSpawn && _enemiesAlive == 0)
            {
                EndWave();
            }

            if(_enemiesAlive == 0)
            {
                EndLevel();
            }
            //end level 

        }

        private void EndLevel()
        {
            Debug.Log("LEVEL ENDED!");
        }

        private int EnemiesPerWave()
        {
            //return Mathf.RoundToInt(_baseCount * Mathf.Pow(_currentWave, difficultyFactor));
            return 0;
        }
    }
}
