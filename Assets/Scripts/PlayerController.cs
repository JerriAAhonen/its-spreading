using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private TilesController tc;
	[SerializeField] private LayerMask obstacleMask;
	[SerializeField] private float obstacleDetectionRadius;
	[SerializeField] private ParticleSystem lanternFF;

	private IInputController ic;
	private PlayerMovement movement;
	private CapsuleCollider capsuleCollider;

	private bool hasFireflies;

	public TilesController TilesController => tc;

	public bool HasFireflies => hasFireflies;
	public float Width => capsuleCollider.radius * 2f;

	public event Action Die;

	private void Awake()
	{
		ic = GetComponent<IInputController>();
		movement = GetComponent<PlayerMovement>();
		movement.Init(ic);
		capsuleCollider = GetComponent<CapsuleCollider>();

		lanternFF.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	private Obstacle currentlySelectedObstacle;

	private void Update()
	{
		Collider closestObstacle = null;
		var pos = transform.position;
		var obstacles = Physics.OverlapSphere(transform.position, obstacleDetectionRadius, obstacleMask);
		if (!obstacles.IsNullOrEmpty())
		{
			float closestDistance = Mathf.Infinity;
			foreach (var obstacleCol in obstacles)
			{
				// Calculate the distance from the center of the sphere to the collider
				float distance = Vector3.Distance(pos, obstacleCol.transform.position);

				// If this distance is smaller than the currently stored minimum distance, update the closest collider and distance
				if (distance < closestDistance)
				{
					closestObstacle = obstacleCol;
					closestDistance = distance;
				}
			}

			var obstacle = closestObstacle.GetComponent<Obstacle>();
			UpdateClosestObstacle(obstacle);
		}
		else
			UpdateClosestObstacle(null);
	}

	public void UpdateClosestObstacle(Obstacle closestObstacle)
	{
		// No obstacles anywhere near
		if (closestObstacle == null && currentlySelectedObstacle == null)
		{
			return;
		}

		// We are no longer near an obstacle
		if (closestObstacle == null && currentlySelectedObstacle != null)
		{
			currentlySelectedObstacle.ActivateOutline(false);
			currentlySelectedObstacle = null;
			return;
		}

		if (closestObstacle != currentlySelectedObstacle)
		{
			if (currentlySelectedObstacle != null)
				currentlySelectedObstacle.ActivateOutline(false);
			
			currentlySelectedObstacle = closestObstacle;
			currentlySelectedObstacle.ActivateOutline(true);
		}
	}

	public void CollectFireflies()
	{
		hasFireflies = true;
		lanternFF.Play();
	}

	public void DepositFireflies()
	{
		hasFireflies = false;
		lanternFF.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	public void CollideWithEnemy()
	{
		Debug.Log("Dead");
		Die?.Invoke();
	}
}
