using UnityEngine;

public class KeyboardAndMouseInput : MonoBehaviour, IInputController
{
	public Vector3 MovementInput { get; private set; }

	private void Update()
	{
		MovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

		// 45 degree rotation matrix
		float cos45 = Mathf.Cos(Mathf.Deg2Rad * 45);
		float sin45 = Mathf.Sin(Mathf.Deg2Rad * 45);

		// Rotate input to match isometric view
		float rotatedX = MovementInput.x * cos45 + MovementInput.z * sin45;
		float rotatedZ = -MovementInput.x * sin45 + MovementInput.z * cos45;
		MovementInput = new Vector3(rotatedX, 0f, rotatedZ);
		MovementInput.Normalize();
	}
}
