using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float movementSpeed;
	[SerializeField] private float rotationSpeed;

	private TilesController tc;
	private IInputController ic;

	public void Init(TilesController tc, IInputController ic)
	{
		this.tc = tc;
		this.ic = ic;
	}

	private Vector3 input = Vector3.zero;
	private Obstacle currentlyPushing;
	private void Update()
	{
		input = ic.MovementInput;
		if (input.Approximately(Vector3.zero)) return;
		if (input.magnitude > 1f) input.Normalize();

		var maxDistanceDelta = movementSpeed * Time.deltaTime * input.magnitude;

		#region ObstaclesWip

		bool? canPush = null;
		// Pushing interaction
		if (Physics.Raycast(transform.position + Vector3.up * 0.25f, transform.forward, out var hit, 0.25f))
		{
			if (hit.collider.gameObject.TryGetComponent(out Obstacle obstacle))
			{
                if (obstacle != currentlyPushing)
				{
					currentlyPushing.OrNull()?.StopPushing();
					currentlyPushing = obstacle;
				}

                if (obstacle.Push(transform.position, maxDistanceDelta / 2f, tc))
				{
					maxDistanceDelta /= 2f;
					canPush = true;
				}
				else
				{
					canPush = false;
				}
			}
			else
				currentlyPushing.OrNull()?.StopPushing();
		}
		else
			currentlyPushing.OrNull()?.StopPushing();

		#endregion

		// Calculate next position from input delta
		var nextPos = Vector3.MoveTowards(transform.position, transform.position + input, maxDistanceDelta);

		// There's an obstacle in front, but it cannot be pushed
		if (canPush.HasValue && canPush == false)
		{
			var dir = (nextPos - transform.position).normalized;
			if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z))
				nextPos = nextPos.With(x: transform.position.x);
			else
				nextPos = nextPos.With(z: transform.position.z);
		}

		// Check if player is moving into a walkable tile, if not, modify the nextPos vector so that it points to a walkable tile
		if (!tc.IsWalkable(nextPos))
		{
			if (tc.IsWalkable(nextPos.With(x: transform.position.x)))
				nextPos = nextPos.With(x: transform.position.x);
			else if (tc.IsWalkable(nextPos.With(z: transform.position.z)))
				nextPos = nextPos.With(z: transform.position.z);
			else
				nextPos = transform.position; // Don't move, there's nowhere to go in the input direction
		}

		// Rotate and move the player towards movement dir
		var finalDir = (nextPos - transform.position).normalized;
		transform.SetPositionAndRotation(
			nextPos,
			Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(finalDir), rotationSpeed * Time.deltaTime));
	}

	private void OnDrawGizmos()
	{

		//Gizmos.DrawSphere(transform.position + input, 0.2f);
	}

	// From tile to tile, Turn based
	/*
	private Vector3 lastTargetPos;
	private Queue<Vector3> targetPositions = new();
	private Coroutine movementRoutine;
	
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			QueueMovement(Vector3.forward);
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			QueueMovement(Vector3.left);
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			QueueMovement(Vector3.back);
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			QueueMovement(Vector3.right);
		}
	}

	private void QueueMovement(Vector3 delta)
	{
		var hasQueuedMovements = targetPositions.Count > 0;
		var previousTargetPos = hasQueuedMovements ? targetPositions.Last() : lastTargetPos;
		var newTargetPos = previousTargetPos + delta;

		if (tiles.IsTile(newTargetPos))
		{
			targetPositions.Enqueue(newTargetPos);
			TryStartMovement();
			lastTargetPos = newTargetPos;
		}
	}

	private void TryStartMovement()
	{
		if (movementRoutine == null && targetPositions.Count > 0)
			movementRoutine = StartCoroutine(MoveToTarget());
	}

	private IEnumerator MoveToTarget()
	{
		var targetPos = targetPositions.Dequeue();

		// TODO Smooth rotation
		var dir = (targetPos - transform.position).normalized;
		transform.rotation = Quaternion.LookRotation(dir);

		while (Vector3.Distance(transform.position, targetPos) > 0.01f)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
			yield return null;
		}
		transform.position = targetPos;
		movementRoutine = null;
		TryStartMovement();
	}
	*/
}
