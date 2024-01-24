using System;
using NaughtyAttributes;
using UnityEngine;

public class LampPost : Interactable
{
	[SerializeField] private MeshRenderer lampRenderer;
	[SerializeField] private Material activeMat;
	[SerializeField] private Material deactiveMat;
	[SerializeField] private AudioEvent depositSFX;

	private bool active;
	private Material lampMat1;
	private Material lampMat2;

	public bool IsLit => active;
	public event Action Lit;

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

		AudioManager.Instance.PlayOnce(depositSFX);

		lampRenderer.materials = new Material[]
			{
				lampMat1, activeMat
			};

		Lit?.Invoke();
	}

	[Button]
	private void EDITOR_Toggle()
	{
		var mats = lampRenderer.sharedMaterials;
		if (mats[1] == activeMat)
			mats[1] = deactiveMat;
		else
			mats[1] = activeMat;

		lampRenderer.sharedMaterials = mats;
	}
}
