// project armada

#pragma warning disable 0414

namespace TowerDefense
{
	using UnityEngine;

	[CreateAssetMenu]
	public class LevelInfo : ScriptableObject
	{
        public WaveInfo[] _waves;
    }
}
	