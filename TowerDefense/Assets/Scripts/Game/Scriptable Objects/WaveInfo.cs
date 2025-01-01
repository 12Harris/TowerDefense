namespace TowerDefense
{
    using UnityEngine;

    [CreateAssetMenu]
	public class WaveInfo : ScriptableObject
	{
        public int _enemyCount;
        public int _enemyType = 0;
        public float _spawnInterval = 0;
    }
}