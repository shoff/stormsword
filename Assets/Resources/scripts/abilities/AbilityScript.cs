﻿using UnityEngine;
using System.Collections;

/* AbilityScript - Base class to use an ability */
public class AbilityScript : MonoBehaviour {

	/* Components */
	private CharacterScript character;    	// Character who has the ability equipped

	public Transform spellEffectPrefab;     		// Effect (Prefab) the ability will apply to players it hits

	[Range (0, 120)]
	public float cooldown = 2.0f; 			// Cooldown between abilities

	[Range (0, 5)]
	public float duration = 0f;				// Time the effect should last (and animate)
	
	public Texture2D abilityIcon;			// The image/icon to display for this ability
	
	private string ownerType;				// Get the tag of the owner to determine if it can damage others

	// Use this for initialization
	void Start () {
		character = GetComponentInParent<CharacterScript>();
		ownerType = character.gameObject.tag; // Get the character's tag so we can decide who the ability should damage

		UseAbility (character.gameObject);
		Destroy(gameObject, duration);	// Effect will go away after (duration) time)
	}
	
	// Update is called once per frame
	void Update () {
	}

	/* UseAbility - Override this with your ability's logic */
	protected virtual void UseAbility(GameObject character) {
	}	
	
	/* ApplySpellEffect - Creates a new effect (DoT, Snare, etc) to a character */
	void ApplySpellEffect(GameObject defender) {
		var effect = Instantiate(spellEffectPrefab) as Transform;
		
		var effectScript = effect.gameObject.GetComponent<Effect>();
		effectScript.target = defender;
		
	}
	
	/* OnTriggerEnter2D - Detect any enemies we collide with and apply an affect (if applicable) */
	void OnTriggerEnter2D(Collider2D defenderCollider) {
		if(spellEffectPrefab != null) {
			// Only check if a spell is equipped
			
			HealthScript defenderHealthScript = defenderCollider.GetComponent<HealthScript>();
			
			// Check if the object I'm colliding with can be damaged
			if(defenderHealthScript != null) {
				
				GameObject defender = defenderHealthScript.gameObject;
				
				if((ownerType == "Player" && defender.tag == "Enemy")
				   ||
				   (ownerType == "Enemy" && defender.tag == "Player")) {
					
					// Apply the spell effect
					ApplySpellEffect (defender);
				}
			}
		}
	}

}
