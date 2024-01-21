using UnityEngine;

public class MobileInput : MonoBehaviour, IInputController
{
	[SerializeField] private Joystick joystick;

	public Vector3 MovementInput { get; private set; }

	private void Update()
	{
		var inputVector = new Vector3(joystick.Direction.x, 0f, joystick.Direction.y);

		// 45 degree rotation matrix
		float cos45 = Mathf.Cos(Mathf.Deg2Rad * 45);
		float sin45 = Mathf.Sin(Mathf.Deg2Rad * 45);

		// Rotate input to match isometric view
		float rotatedX = inputVector.x * cos45 + inputVector.z * sin45;
		float rotatedZ = -inputVector.x * sin45 + inputVector.z * cos45;
		MovementInput = new Vector3(rotatedX, inputVector.y, rotatedZ);
	}
}
