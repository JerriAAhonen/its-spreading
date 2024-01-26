using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private TilesController tc;
	[SerializeField] private LayerMask obstacleMask;
	[SerializeField] private float obstacleDetectionRadius;
	[SerializeField] private ParticleSystem lanternFF;
	[Header("Death")]
	[SerializeField] private float deathAnimDur;
	[SerializeField] private ParticleSystem deathPS;
	[SerializeField] private GameObject bodyModel;
	[SerializeField] private GameObject lantern;
	[SerializeField] private HingeJoint lanternHinge;
	[SerializeField] private MeshRenderer lampRenderer;
	[SerializeField] private Material glassUnlit;
	[SerializeField] private Light lanternLight;
	[SerializeField] private ParticleSystem escapingFF;
	[Space]
	[SerializeField] private GameObject tutorialCanvas;
	[SerializeField] private TextMeshProUGUI tutorialLabel;

	private IInputController ic;
	private PlayerMovement movement;
	private CapsuleCollider capsuleCollider;

	private bool alive = true;
	private bool hasFireflies;

	private Material lampMat1;

	public TilesController TilesController => tc;
	public Transform LanternMiddle => lanternLight.transform;

	public bool MovementEnabled { get; private set; }
	public bool HasFireflies => hasFireflies;
	public float Width => capsuleCollider.radius * 2f;

	public event Action Die;

	private void Awake()
	{
		ic = GetComponent<IInputController>();
		movement = GetComponent<PlayerMovement>();
		movement.Init(ic, this);
		capsuleCollider = GetComponent<CapsuleCollider>();
		MovementEnabled = true;

		lanternFF.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		lampMat1 = lampRenderer.materials[0];
	}

	private HashSet<Collider> oldObsCols = new();

	private void Update()
	{
		var newObsCols = Physics.OverlapSphere(transform.position, obstacleDetectionRadius, obstacleMask);
		if (!newObsCols.IsNullOrEmpty())
		{
			foreach (var newCol in newObsCols)
				if (!oldObsCols.Contains(newCol))
					newCol.GetComponent<Obstacle>().ActivateOutline(true);

			foreach (var oldCol in oldObsCols)
				if (!newObsCols.Contains(oldCol))
					oldCol.GetComponent<Obstacle>().ActivateOutline(false);

			oldObsCols.Clear();
			foreach (var obsCol in newObsCols)
				oldObsCols.Add(obsCol);
		}
		else
		{
			foreach(var oldCol in oldObsCols)
				oldCol.GetComponent<Obstacle>().ActivateOutline(false);
			oldObsCols.Clear();
		}
	}

	public void CollectFireflies()
	{
		hasFireflies = true;
		lanternFF.Play();
	}

	public void DepositFireflies()
	{
		hasFireflies = false;
		lanternFF.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	public void CollideWithEnemy()
	{
		if (!alive) return;
		alive = false;

		Debug.Log("Dead");
		DisableControls();
		AnimateDeath();
		LeanTween.delayedCall(deathAnimDur, () => 
		{ 
			if (this != null)
				Die?.Invoke();
		});
	}

	public void ShowTutorialText(string text)
	{
		if (text.IsNullOrEmpty())
		{
			tutorialCanvas.SetActive(false);
			return;
		}

		tutorialLabel.text = "";
		tutorialCanvas.SetActive(true);

		StartCoroutine(Routine());
		

		IEnumerator Routine()
		{
			foreach (char character in text.ToCharArray())
			{
				tutorialLabel.text += character;
				yield return null;
				yield return null;
			}

			LeanTween.delayedCall(5f, () => 
			{
				if (this != null)
					tutorialCanvas.SetActive(false); 
			});
		}
	}

	private void DisableControls()
	{
		MovementEnabled = false;
	}

	private void AnimateDeath()
	{
		bodyModel.SetActive(false);
		deathPS.Play();
		Destroy(lanternHinge);
		lantern.transform.SetParent(transform.parent);


		lampRenderer.materials = new Material[]
			{
				lampMat1, glassUnlit
			};

		LeanTween.value(lanternLight.intensity, 0f, deathAnimDur)
			.setOnUpdate(v => lanternLight.intensity = v);

		// Animate fireflies escaping
		if (hasFireflies)
		{
			lanternFF.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			escapingFF.Play();
		}
	}
}
