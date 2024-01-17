using UnityEngine;

public class InputController : MonoBehaviour
{
	public Vector3 MovementInput { get; private set; }

	private void Update()
	{
		MovementInput = Vector3.zero;

		if (Input.GetKey(KeyCode.W))
		{
			MovementInput += Vector3.forward;
		}
		if (Input.GetKey(KeyCode.A))
		{
			MovementInput += Vector3.left;
		}
		if (Input.GetKey(KeyCode.S))
		{
			MovementInput += Vector3.back;
		}
		if (Input.GetKey(KeyCode.D))
		{
			MovementInput += Vector3.right;
		}
	}
}
