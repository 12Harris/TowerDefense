// project armada

#pragma warning disable 0414

namespace Harris
{
	using UnityEngine;

	[CreateAssetMenu]
	public class CameraData : ScriptableObject
	{
		[Header("Camera Data")]
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 70.0f;
		public float BottomClamp = -30.0f;
		public float CameraAngleOverride = 0.0f;
		public bool LockCameraPosition = false;	

		public float _threshold = 0.01f;

		// cinemachine
		public float _cinemachineTargetYaw;
        public float _cinemachineTargetPitch;

	}
}