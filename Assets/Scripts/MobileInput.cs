using UnityEngine;

public class MobileInput : MonoBehaviour, IInputController
{
	[SerializeField] private Joystick joystick;

	public Vector3 MovementInput { get; private set; }

	private void Update()
	{
		MovementInput = new Vector3(joystick.Direction.x, 0f, joystick.Direction.y);
		Debug.Log(MovementInput);
	}
}
