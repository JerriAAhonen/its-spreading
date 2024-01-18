using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float movementSpeed;

	private TilesController tc;
	private IInputController ic;

	public void Init(TilesController tc, IInputController ic)
	{
		this.tc = tc;
		this.ic = ic;
	}

	private Vector3 input = Vector3.zero;
	private void Update()
	{
		input = ic.MovementInput.normalized;
		var roundedInput = new Vector3(Mathf.Round(input.x), 0f, Mathf.Round(input.z));
		Debug.Log(roundedInput);

		transform.position = Vector3.MoveTowards(transform.position, transform.position + roundedInput, movementSpeed * Time.deltaTime);
	}

	private void OnDrawGizmos()
	{
		
		Gizmos.DrawSphere(transform.position + input, 0.2f);
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
