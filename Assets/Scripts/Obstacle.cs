using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	[SerializeField] private LayerMask playerLayer;
	[SerializeField] private LayerMask obstacleLayer;
	[SerializeField] private Outline outline;
	[SerializeField] private float moveDur;
	[SerializeField] private float thumpDur;
	[SerializeField] private AudioEvent pushSFX;
	[SerializeField] private AudioEvent thumpSFX;
	[SerializeField] private ParticleSystem thumpPS;

	private bool lockedInPlace; // Is the obstacle pushed into a waterTile?
	private bool isMoving;

	private void Awake()
	{
		outline.OutlineWidth = 0f;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (lockedInPlace) return;
		if (isMoving) return;

		if (BitMaskUtil.MaskContainsLayer(playerLayer, collision.collider.gameObject.layer))
		{
			Debug.Log("Hit player");
			var tc = collision.collider.gameObject.GetComponent<PlayerController>().TilesController;

			var pushedFrom = collision.collider.transform.position;
			var dir = transform.position - pushedFrom;
			dir.x = Mathf.RoundToInt(dir.x);
			dir.z = dir.x == 0 ? Mathf.RoundToInt(dir.z) : 0; // If X-dir is set, don't move in Z-dir aswell. Only one dir at a time! >:(
			dir.y = 0f;

			if (Physics.Raycast(transform.position, dir, out var hit, 1f, obstacleLayer))
			{
				Debug.Log("Can't push into another stone");
				return;
			}

			var targetPos = transform.position + dir;
			if (tc.CanPushObstacleInto(targetPos, out var isWater))
			{
				isMoving = true;
				AudioManager.Instance.PlayOnce(pushSFX);

				LeanTween.move(gameObject, targetPos, moveDur)
					.setEase(LeanTweenType.easeOutQuad)
					.setOnComplete(() =>
					{
						if (isWater)
						{
							tc.UpdateGrid_HideTile(targetPos);

							targetPos = targetPos.With(y: 0f);
							LeanTween.move(gameObject, targetPos, thumpDur)
								.setEase(LeanTweenType.easeInOutQuart)
								.setOnComplete(() =>
								{
									// Thump
									AudioManager.Instance.PlayOnce(thumpSFX);
									// Dust
									thumpPS.Play();

									isMoving = false;
								});
						}
						else
							isMoving = false;
					});

				if (isWater)
				{
					// Update tiles colliders
					//tc.RemoveCollider(targetPos);
					tc.UpdateGrid_NewSolid(targetPos);
					lockedInPlace = true;
				}
			}
		}
	}

	public void ActivateOutline(bool activate)
	{
		if (lockedInPlace)
		{
			outline.OutlineWidth = 0f;
			return;
		}

		outline.OutlineWidth = activate ? 1f : 0f;
	}
}

/*
 find dir to next tile
move towards only that tile or previous tile
when reached center of the tile, recalculate dir
check that next tile in walkable
snap player to push position
 
 */