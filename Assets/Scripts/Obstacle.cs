using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	[SerializeField] private LayerMask playerLayer;
	[SerializeField] private Outline outline;
	//[SerializeField] private ;

	private Rigidbody rb;

	private bool lockedInPlace; // Is the obstacle pushed into a waterTile?

	private void Awake()
	{
		outline.OutlineWidth = 0f;
		//rb = GetComponent<Rigidbody>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (lockedInPlace) return;

		if (BitMaskUtil.MaskContainsLayer(playerLayer, collision.collider.gameObject.layer))
		{
			Debug.Log("Hit player");
			var tc = collision.collider.gameObject.GetComponent<PlayerController>().TilesController;

			var pushedFrom = collision.collider.transform.position;
			var dir = transform.position - pushedFrom;
			dir.x = Mathf.RoundToInt(dir.x);
			dir.z = Mathf.RoundToInt(dir.z);
			dir.y = 0f;

			var targetPos = transform.position + dir;
			if (tc.CanPushObstacleInto(targetPos, out var isWater))
			{
				targetPos = isWater 
					? targetPos.With(y:0f) 
					: targetPos;
				transform.position = targetPos;

				if (isWater)
				{
					// Update tiles colliders
					tc.RemoveCollider(targetPos);
					lockedInPlace = true;
				}
			}
		}
	}
}

/*
 find dir to next tile
move towards only that tile or previous tile
when reached center of the tile, recalculate dir
check that next tile in walkable
snap player to push position
 
 */