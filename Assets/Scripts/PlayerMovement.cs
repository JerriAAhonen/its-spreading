using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float movementSpeed;
	[SerializeField] private float rotationSpeed;
	[SerializeField] private Transform model;
	[SerializeField] private float animationSpeed;
	[SerializeField] private float animationHeightMult;

	private IInputController ic;
	private Rigidbody rb;

	public void Init(IInputController ic)
	{
		this.ic = ic;
		rb = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		var input = ic.MovementInput.normalized;
		rb.velocity = movementSpeed * Time.deltaTime * input;
		if (!input.Approximately(Vector3.zero))
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(input), rotationSpeed * Time.deltaTime);
			// Animation
			var bounceOffset = Mathf.Abs(Mathf.Sin(Time.time * animationSpeed)) * animationHeightMult;
			model.transform.localPosition = new Vector3(0f, bounceOffset, 0f);
		}
		else
			model.transform.localPosition = Vector3.MoveTowards(model.transform.localPosition, new Vector3(0f, 0f, 0f), Time.deltaTime);
	}

}
