using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : Interactable
{
	[SerializeField] private MeshRenderer lampRenderer;
	[SerializeField] private Material activeMat;
	[SerializeField] private Material deactiveMat;

	private bool active;
	private Material lampMat1;
	private Material lampMat2;

	private void Awake()
	{
		active = false;
		lampMat1 = lampRenderer.materials[0];
		lampMat2 = lampRenderer.materials[1];
		lampRenderer.materials = new Material[]
			{
				lampMat1, deactiveMat
			};
	}

	protected override void OnInteract(PlayerController pc)
	{
		if (active) return;
		if (!pc.HasFireflies) return;

		active = true;
		pc.DepositFireflies();

		lampRenderer.materials = new Material[]
			{
				lampMat1, activeMat
			};
	}
}
