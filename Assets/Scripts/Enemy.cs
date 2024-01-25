using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public enum PathFollowType
{
	None,
	PingPong,
	Loop
}

public class Enemy : MonoBehaviour
{
	[SerializeField] private PathFollowType pathFollowType;
	[SerializeField] private List<Transform> path;
	[SerializeField] private bool DEBUG_DrawPath;
	[SerializeField] private LayerMask playerLayer;
	[Space]
	[SerializeField] private float movementSpeed;
	[SerializeField] private float rotationSpeed;
	[SerializeField] private Transform model;
	[SerializeField] private float animationSpeed;
	[SerializeField] private float animationHeightMult;
	[Space]
	[SerializeField] private ParticleSystem deathPS;
	[SerializeField] private ParticleSystem blackSmokePS;

	private bool hasPath;
	private bool forward;
	private int currentTargetIndex;
	private Transform target;

	private void Start()
	{
		hasPath = !path.IsNullOrEmpty() && path.Count > 1;
		if (!hasPath) return;

		forward = true;
		currentTargetIndex = 1;
		target = path[currentTargetIndex];
	}

	private void Update()
	{
		if (!hasPath) return; // Just do idle animation
		if (pathFollowType == PathFollowType.None) return;

		// Move towards target
		// If obstacle, turn around
		var nextPos = Vector3.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
		var dir = (nextPos - transform.position).normalized;

		transform.position = nextPos;
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);

		if (transform.position.InRangeOf(target.position, 0.01f))
		{
			SetNextTarget();
		}

		var bounceOffset = Mathf.Sin((Time.time) * animationSpeed) * animationHeightMult;
		model.transform.localPosition = new Vector3(0f, bounceOffset, 0f);
	}

	private void SetNextTarget()
	{
		if (pathFollowType == PathFollowType.PingPong)
		{
			// Next target
			if (forward)
			{
				if (currentTargetIndex == path.Count - 1)
				{
					forward = false;
					currentTargetIndex--;
				}
				else
					currentTargetIndex++;
			}
			else
			{
				if (currentTargetIndex == 0)
				{
					forward = true;
					currentTargetIndex++;
				}
				else
					currentTargetIndex--;
			}
		}
		else if (pathFollowType == PathFollowType.Loop)
		{
			if (forward)
			{
				currentTargetIndex++;
				if (currentTargetIndex >= path.Count)
					currentTargetIndex = 0;
			}
			else
			{
				currentTargetIndex--;
				if (currentTargetIndex < 0)
					currentTargetIndex = path.Count - 1;
			}
		}

		target = path[currentTargetIndex];
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent<PlayerController>(out var player))
			player.CollideWithEnemy();

		if (other.gameObject.TryGetComponent<Obstacle>(out var obstacle))
		{
			forward = !forward;
			SetNextTarget();
		}
	}

	private void OnDrawGizmos()
	{
		if (!DEBUG_DrawPath) return;
		if (pathFollowType == PathFollowType.None) return;
		if (path.IsNullOrEmpty() || path.Count == 1) return;

		for (int i = 0; i < path.Count; i++)
		{
			if (i < path.Count - 1)
			{
				Gizmos.DrawLine(path[i].position, path[i+1].position);
			}

			Gizmos.DrawSphere(path[i].position, 0.1f);
		}

		if (pathFollowType == PathFollowType.Loop)
			Gizmos.DrawLine(path[0].position, path[^1].position);
    }

	// Called when all the lights are lit!
	public void Die()
	{
		hasPath = false; // Stops the enemy from moving
		model.gameObject.SetActive(false);
		deathPS.Play();
		blackSmokePS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	[Button]
	private void EDITOR_CreatePathNode()
	{
		var node = new GameObject("PathNode");
		node.transform.position = transform.position;
		node.transform.parent = transform.parent;
		path.Add(node.transform);
	}
}
