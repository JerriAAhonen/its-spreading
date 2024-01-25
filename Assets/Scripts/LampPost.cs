using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class LampPost : Interactable
{
	[SerializeField] private MeshRenderer lampRenderer;
	[SerializeField] private Material activeMat;
	[SerializeField] private Material deactiveMat;
	[SerializeField] private Light pointLight;
	[SerializeField] private ParticleSystem lanternFF;
	[SerializeField] private AudioEvent depositSFX;
	[SerializeField] private Animator lanternAnimator;

	private bool active;
	private Material lampMat1;

	public bool IsLit => active;
	public event Action Lit;

	private void Awake()
	{
		active = false;
		pointLight.enabled = false;
		lanternFF.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		lampMat1 = lampRenderer.materials[0];
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

		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			pointLight.intensity = 0f;
			pointLight.enabled = true;
			lanternFF.Play();
			lanternAnimator.SetTrigger("Deposit");
			LeanTween.value(0f, 0.4f, 2f)
				.setOnUpdate(v => pointLight.intensity = v)
				.setEase(LeanTweenType.easeOutCubic);
			yield return WaitForUtil.RealSeconds(2f);
			Lit?.Invoke();
		}
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
