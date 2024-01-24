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
	// TODO Setup path in editor
	// TODO Move whenever the player moves OR move on it's own
	// TODO Kill the player when touch

	[SerializeField] private PathFollowType pathFollowType;
	[SerializeField] private List<Transform> path;
	[SerializeField] private bool DEBUG_DrawPath;
	[SerializeField] private LayerMask playerLayer;

	[SerializeField] private float movementSpeed;
	[SerializeField] private float rotationSpeed;

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
		transform.position = nextPos;

		var dir = (nextPos - transform.position).normalized;
		if (!dir.Approximately(Vector3.zero))
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.deltaTime);

		if (transform.position.InRangeOf(target.position, 0.01f))
		{
			SetNextTarget();
		}
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

	[Button]
	private void EDITOR_CreatePathNode()
	{
		var node = new GameObject("PathNode");
		node.transform.position = transform.position;
		node.transform.parent = transform.parent;
		path.Add(node.transform);
	}
}
