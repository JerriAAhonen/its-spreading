using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireflies : MonoBehaviour
{
	[SerializeField] private float pickupDur;
	[Range(0f, 1f)]
	[SerializeField] private float turnAroundPoint;
	[SerializeField] private float accelerationRateOut;
	[SerializeField] private float accelerationRateIn;

	[SerializeField] private ParticleSystem ps;
	[SerializeField] private ParticleSystem destroyedPs;
	[SerializeField] new private Light light;
	[SerializeField] private AudioEvent pickupSFX;

	private bool active = true;

	public event Action Destroyed;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.TryGetComponent<PlayerController>(out var pc))
		{
			if (!active) return;
			if (pc.HasFireflies) return;

			active = false;
			pc.CollectFireflies();

			AudioManager.Instance.PlayOnce(pickupSFX);

			//ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			StartCoroutine(PickupRoutine());
		}

		if (other.gameObject.TryGetComponent<Obstacle>(out _))
		{
			ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			destroyedPs.Play();
			Destroyed?.Invoke();
		}
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
		gameObject.SetActive(false);
	}
}
