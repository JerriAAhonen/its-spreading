using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float movementSpeed;
	[SerializeField] private float rotationSpeed;
	[SerializeField] private Transform model;
	[SerializeField] private float animationSpeed;
	[SerializeField] private float animationHeightMult;
	[SerializeField] private AudioEvent walkSFX;

	private IInputController ic;
	private Rigidbody rb;
	private bool goingUp;
	private bool directionChanged;
	private float prevHeight;

	public void Init(IInputController ic)
	{
		this.ic = ic;
		rb = GetComponent<Rigidbody>();
	}

	private float startMovingTime;

	private void FixedUpdate()
	{
		var input = ic.MovementInput.normalized;
		rb.velocity = movementSpeed * Time.deltaTime * input;
		if (!input.Approximately(Vector3.zero))
		{
			if (startMovingTime < 0)
			{
				startMovingTime = Time.time;
			}

			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(input), rotationSpeed * Time.deltaTime);
			
			// Animation
			var bounceOffset = Mathf.Abs(Mathf.Sin((Time.time - startMovingTime) * animationSpeed)) * animationHeightMult;
			model.transform.localPosition = new Vector3(0f, bounceOffset, 0f);

			if (goingUp != bounceOffset > prevHeight)
			{
				goingUp = bounceOffset > prevHeight;

				if (goingUp)
				{
					AudioManager.Instance.PlayOnce(walkSFX);
				}
			}

			prevHeight = bounceOffset;
		}
		else
		{
			model.transform.localPosition = Vector3.MoveTowards(model.transform.localPosition, new Vector3(0f, 0f, 0f), Time.deltaTime);
			startMovingTime = -1f;
		}
	}

}
