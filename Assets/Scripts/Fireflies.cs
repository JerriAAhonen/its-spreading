using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireflies : Interactable
{
	[SerializeField] private float pickupDur;
	[Range(0f, 1f)]
	[SerializeField] private float turnAroundPoint;
	[SerializeField] private float accelerationRateOut;
	[SerializeField] private float accelerationRateIn;

	[SerializeField] private ParticleSystem ps;
	[SerializeField] new private Light light;

	private bool active;

	private void Awake()
	{
		active = true;
	}

	protected override void OnInteract(PlayerController pc)
	{
		if (!active) return;
		if (pc.HasFireflies) return;

		active = false;
		pc.CollectFireflies();

		//ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
		StartCoroutine(PickupRoutine());
	}

	private IEnumerator PickupRoutine()
	{
		float elapsed = 0f;
		float turnAroundAfter = pickupDur * turnAroundPoint;
		bool movingOutward = true;

		var VelOverLifetime = ps.velocityOverLifetime;
		var radial = 0f;

		while (elapsed < pickupDur)
		{
			elapsed += Time.deltaTime;
			yield return null;

			if (elapsed >= turnAroundAfter)
				movingOutward = false;

			var radialDelta = movingOutward
				? accelerationRateOut * Time.deltaTime
				: accelerationRateIn * Time.deltaTime;
			radial += radialDelta;

			VelOverLifetime.radialMultiplier = radial;
			//Debug.Log(VelOverLifetime.radialMultiplier);

			// Light
			if (movingOutward)
			{
				light.intensity += Time.deltaTime;
			}
			else
			{
				light.intensity -= Time.deltaTime;
			}
		}

		ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		light.enabled = false;
	}
}
