using UnityEngine;
using AIStates;
using System.Security.Cryptography;
using System.Collections.Specialized;

namespace Harris.GPC
{
	using Harris.Util;
	using System;

	public class AIBotController : BaseEnemyStatsController
	{
		private Vector3 moveVec;
		private Vector3 targetMoveVec;
		public float distanceToChaseTarget;

		[Header("Movement")]
		public float moveSpeed = 30f;

		public bool faceWaypoints;

		public Rigidbody rb;

		[SerializeField]
		private Transform directlyInFront;

		private bool stopMovement;

		private Vector3 targetPosition;

		[SerializeField]
		public WaypointsController home_waypointsController;

		[SerializeField]
		private ObstacleWaypointsController obs_waypointsController;

		public bool avoidObstacle = false;

		protected Animator animator;
		public Animator Animator => animator;

		protected bool hasAnimator;
		public bool HasAnimator => hasAnimator;

		public int animIDSpeed = 0;
		public int animIDMotionSpeed = 0;
		
	public virtual void Awake()
	{
		animator = GetComponent<Animator>();
		hasAnimator = animator != null;
	}
	public virtual void Start()
	{

		base.Start();
		rb = GetComponent<Rigidbody>();
		animator = GetComponent<Animator>();
		stopMovement = false;

		SetWayController(home_waypointsController);

		_onReachedNextWaypoint += handleReachedNextWaypoint;
		_onReachedLastWaypoint += handleReachedLastWaypoint;

		AssignAnimationIDs();
	}

		private void AssignAnimationIDs()
		{
			animIDSpeed = Animator.StringToHash("Speed");
			animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
		}

		public void MoveHome()
		{
			SetWayController(home_waypointsController);
			SetAIState(AIState.move_along_waypoint_path);
		}

		private void handleReachedNextWaypoint()
		{
			
		}

		private void handleReachedLastWaypoint()
		{
			if (_waypointsController != home_waypointsController && !CanSee(_followTarget)) // and ! cansee player
			{
				SetWayController(home_waypointsController);
				shouldReversePathFollowing = false;
			}
		}

		public override void Update()
		{
			base.Update();
			distanceToChaseTarget = Vector3.Distance(_TR.position, _followTarget.position);
		}

		public void SetAvoidObstacleFlag()
		{
			avoidObstacle = true;
		}


		/// <summary>
		/// Finds the starting waypoint needed to avoid collision with obstacle
		/// </summary>
		public void AvoidObstacle()
		{

			if(obstacleFinderResult != 0 && obstacleFinderResult != 3)
			{
				_currentWaypointTransform = findStartingWaypoint(out currentWaypointNum);
				Debug.Log("start wp = " + _currentWaypointTransform.gameObject);
				
				if (_currentWaypointTransform != null)
				{
					SetWayController(_currentWaypointTransform.parent.gameObject.GetComponent<WaypointsController>(), false);
				}

				if(obstacleFinderResult == 1)			//go right
					shouldReversePathFollowing = true;
				else									//go left
					shouldReversePathFollowing = false;

			}
		}

		public override void UpdateCurrentState()
		{

			distanceToChaseTarget = Vector3.Distance(_TR.position, _followTarget.position);

			/*Collider[] hitColliders = Physics.OverlapSphere(_followTarget.position, 1f, obstacleAvoidLayers);

			if (hitColliders.Length > 0)
			{
				Debug.Log("target near obstacle!");
			}*/

			obstacleFinderResult = IsObstacleAhead();

		}

		public virtual bool AttackTarget()
		{
			rb.velocity = Vector3.zero;
			if (hasAnimator)
			{
				animator.SetFloat(animIDSpeed, 0f);
				animator.SetFloat(animIDMotionSpeed,1);
			}
			return obstacleFinderResult == 0 && InAttackingDistance();
		}

		public virtual bool InAttackingDistance()
		{
			return distanceToChaseTarget < minChaseDistance + 0.1f;
		}

		public void ChasingTarget()
		{
			TurnTowardTarget(_followTarget);

			avoidObstacle = false;

			rb.velocity = transform.forward * moveSpeed;

			if (hasAnimator)
			{
				animator.SetFloat(animIDSpeed, moveSpeed);
				animator.SetFloat(animIDMotionSpeed, 1);
			}
		}

		private void PausedNoTarget()
		{
			stopMovement = true;
		}

		public void MoveAlongWaypointPath()
		{

			// make sure we have been initialized before trying to access waypoints
			if (!didInit && !reachedLastWaypoint)
			{
				return;
			}

			UpdateWaypoints();

			var v = _currentWaypointTransform.position;
			v.y = _TR.position.y;

			targetMoveVec = Vector3.Normalize(v - _TR.position);
			moveVec = Vector3.Lerp(moveVec, targetMoveVec, Time.deltaTime * pathSmoothing);
			//_TR.Translate(moveVec * moveSpeed * Time.deltaTime, Space.World);

			moveVec = targetMoveVec;

			if (distanceToChaseTarget > minChaseDistance)
			{
				rb.velocity = moveVec * moveSpeed;

				if (hasAnimator)
				{
					animator.SetFloat(animIDSpeed, moveSpeed);
					animator.SetFloat(animIDMotionSpeed, 1);
				}
			}
			else
			{
				rb.velocity = Vector3.zero;
				if (hasAnimator)
				{
					animator.SetFloat(animIDSpeed, 0f);
					animator.SetFloat(animIDMotionSpeed, 1);
				}
			}

			if (faceWaypoints)
			{
				Debug.Log("face waypoint!");
				TurnTowardTarget(_currentWaypointTransform);
			}
		
		}

		public void SetMoveSpeed(float aNum)
		{
			moveSpeed = aNum;
		}
	}
}