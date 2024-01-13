using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private TilesController tiles;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			Move(Vector3.forward);
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			Move(Vector3.left);
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			Move(Vector3.back);
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			Move(Vector3.right);
		}
	}

	private void Move(Vector3 delta)
	{
		transform.position += delta;
		if (!tiles.IsTile(new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z)))
		{
			Debug.Log("Game over!");
		}
	}
}
