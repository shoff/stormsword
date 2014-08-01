﻿using UnityEngine;
using System.Collections;

public class AbilityScript : MonoBehaviour {

	/* Components */
	private AbilitySlotScript abilitySlot;	// Get the ability slot that this ability is equipped in


	public float cooldown = 2.0f; // Cooldown between abilities


	public Transform abilityEffect;	// Effect animation triggered when ability is used

	// Use this for initialization
	void Start () {
		abilitySlot = transform.parent.GetComponent<AbilitySlotScript>();	// Grab the parent ability to get any slot-related info
	}
	
	// Update is called once per frame
	void Update () {
	}

	/* Cast - Trigger the current ability (if off cooldown) */
	internal void Cast() {
		if(abilitySlot.CanCast) {
			// Used ability, trigger cooldown
			abilitySlot.Cooldown(cooldown);

			var abilityTransform = Instantiate(abilityEffect) as Transform;
			abilityTransform.transform.parent = transform;	// Effect should be a child of the ability slot (and thus the Character)
			abilityTransform.transform.position = transform.position;	// Effect should spawn underneath player
		}
	}
}
