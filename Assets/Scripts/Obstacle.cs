using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	[SerializeField] private Outline outline;
	//[SerializeField] private ;

	private void Awake()
	{
		outline.OutlineWidth = 0f;
	}

	public bool Push(Vector3 pushedFrom, float maxDistanceDelta, TilesController tc)
	{
		outline.OutlineWidth = 2f;

		var dir = transform.position - pushedFrom;
		dir.x = Mathf.RoundToInt(dir.x);
		dir.z = Mathf.RoundToInt(dir.z);
		dir.y = 0f;

		var targetPos = transform.position + dir;
		if (tc.CanPushObstacleInto(targetPos))
		{
			transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, maxDistanceDelta);
			return true;
		}
		else
			return false;
	}

	public void StopPushing()
	{
		outline.OutlineWidth = 0f;
	}
}

/*
 find dir to next tile
move towards only that tile or previous tile
when reached center of the tile, recalculate dir
check that next tile in walkable
snap player to push position
 
 */