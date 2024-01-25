using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class LampPost : MonoBehaviour
{
	[SerializeField] private MeshRenderer lampRenderer;
	[SerializeField] private Material activeMat;
	[SerializeField] private Material deactiveMat;
	[SerializeField] private Transform lightTargetPos;
	[SerializeField] private Light pointLight;
	[SerializeField] private ParticleSystem lanternFF;
	[SerializeField] private AudioEvent startDepositSFX;
	[SerializeField] private AudioEvent depositSFX;
	[SerializeField] private Animator lanternAnimator;
	[SerializeField] private ParticleSystem fromPlayerToLampFF;

	private bool active;
	private Material lampMat1;

	public bool IsLit => active;
	public event Action Lit;

	private void Awake()
	{
		active = false;
		pointLight.enabled = false;
		lanternFF.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		fromPlayerToLampFF.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		lampMat1 = lampRenderer.materials[0];
		lampRenderer.materials = new Material[]
			{
				lampMat1, deactiveMat
			};
	}

	private void OnTriggerEnter(Collider other)
	{
		if (active) return;

		if (other.gameObject.TryGetComponent<PlayerController>(out var pc))
		{
			if (!pc.HasFireflies) return;

			active = true;
			pc.DepositFireflies();

			AudioManager.Instance.PlayOnce(startDepositSFX);

			StartCoroutine(Routine());
		}

		IEnumerator Routine()
		{
			pointLight.intensity = 0.4f;
			pointLight.enabled = true;

			var fromPos = pc.LanternMiddle.position;
			var toPos = pointLight.transform.position;

			pointLight.transform.position = fromPos;
			fromPlayerToLampFF.Play(true);

			LeanTween.move(pointLight.gameObject, toPos, 1f)
				.setOnComplete(() =>
				{
					fromPlayerToLampFF.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
					
					lanternFF.Play();
					lanternAnimator.SetTrigger("Deposit");
					AudioManager.Instance.PlayOnce(depositSFX);
					lampRenderer.materials = new Material[]
					{
						lampMat1, activeMat
					};
				});

			yield return WaitForUtil.RealSeconds(2f);
			Lit?.Invoke();
		}
	}
}
