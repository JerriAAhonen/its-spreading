using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

public class EditorUtil_PositionPlayerCamera : MonoBehaviour
{
	[SerializeField] private Vector3 offset;

	[Button]
	private void Setup()
	{
		var playerTransform = transform.parent.GetComponentInChildren<PlayerController>().transform;
		var playerPos = playerTransform.position;
		transform.position = playerPos + offset;

		var vCam = GetComponent<CinemachineVirtualCamera>();
		vCam.Follow = playerTransform;
		vCam.LookAt = playerTransform;
	}
}
