﻿using UnityEngine;
using System.Collections;

/* WeaponScript - Basic weapon class (Ranged or Melee) */

public class WeaponScript : MonoBehaviour {

	// Projectile prefab for shooting
	public Transform shotPrefab;
	
	// Weapon stats
	[Tooltip("Cooldown between attacks")]
	public float shootingRate = 0.25f;

	[Tooltip("Damage a weapon does per attack")]
	public float damage = 1;

	[Tooltip("Radius the weapon affects upon impact")]
	public float radius = 5;

	[Tooltip("Amount to knock the character back upon impact")]
	public float knockback = 500f;

	[Tooltip("Is this a Melee or Ranged weapon?")]
	public string type = "Melee";


	public string ownerType = "Player";	// Is a player or an enemy carrying the weapon?

	// Components
	private MoveScript parentMoveScript;
	private MoveScript shotMoveScript;
	private ItemSlotScript mainHandSlot;

	private Vector2 aiPosition;
	private Vector2 playerPosition;
	private Vector2 newDirection;
	private float playerDistance;

	private GameObject player;

	// Use this for initialization
	void Start () {
		mainHandSlot = transform.parent.GetComponent<ItemSlotScript>();	// Grab the parent mainhand to get any slot-related info
	}

	void Update () {
	}

	/* Attack - Shot triggered by another script */
	public void Attack() {
		if(CanAttack) {
			// Character attacked, trigger cooldown
			mainHandSlot.Cooldown(shootingRate);

 			switch(type) {
			case "Melee": 
				// Handle melee weapon code here

				// Melee attack is attached to parent (character)

				break;

			case "Ranged": 
				// Handle ranged weapon code here

				player = GameObject.FindWithTag("Player");
				// Is the player still alive?
				if(player != null)
				{

					//Shoot directly at player location
					aiPosition = new Vector2(transform.position.x , transform.position.y);
					playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
					newDirection = (playerPosition - aiPosition);


					playerDistance = Vector3.Distance(player.transform.position, aiPosition);

						// Player is within shot distance
						if(playerDistance <=5f)
						{

							// Create a new shot
							var shotTransform = Instantiate(shotPrefab) as Transform;


							parentMoveScript = transform.parent.parent.GetComponent<MoveScript>();

							// Shot transform should be a child of the item slot (and thus the Character)
							shotTransform.transform.parent = transform;	

							if(parentMoveScript)
							{

								// Spawn projectile under character
								shotTransform.position = transform.position;

								// Figure out what direction character is facing
								//Vector3 newDirection = new Vector3(parentMoveScript.facing.x, parentMoveScript.facing.y, 0);

								//Shoot directly at player location
								aiPosition = new Vector2(transform.position.x , transform.position.y);
								playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
								newDirection = (playerPosition - aiPosition);

								// Make sure the bullet isn't going way too fast
								newDirection = newDirection.normalized;

								// Get the shot's move script to adjust its direction
								shotMoveScript = shotTransform.GetComponent<MoveScript>();
						
								if(shotMoveScript) {
									shotMoveScript.direction = newDirection;
								}
						
							// Fire the actual projectile
							ProjectileScript projectile = shotTransform.gameObject.GetComponent<ProjectileScript>();

								if(projectile) {
									// Determine what type of player shot this
									projectile.ownerType = gameObject.transform.parent.parent.tag;
								}
							}
						}
				}
				break;	

			}


		}
	}

	// Is the weapon ready to create a new projectile?
	public bool CanAttack {
		get {
			return(mainHandSlot.attackCooldown <= 0f);
		}
	}

	void OnTriggerEnter2D(Collider2D defenderCollider) {

		HealthScript defenderHealth = defenderCollider.GetComponent<HealthScript>();

		// Check if the object I'm colliding with can be damaged
		if(defenderHealth != null) {

			GameObject defender = defenderHealth.gameObject;

			if((ownerType == "Player" && defender.tag == "Enemy")
			   ||
			   (ownerType == "Enemy" && defender.tag == "Player")
			   || 
			   (ownerType == "Player" && defender.tag == "Terrain")){

				// Attacks should knock the defender back away from attacker
				Knockback (transform, defender, knockback);

				// Calculate armor reduction
				float totalDamage = damage - defenderHealth.armor;

				// attacks should always do 1 dmg, even if they are very weak
				if(totalDamage <= 0) {
					totalDamage = 1;
				}

				defenderHealth.Damage(totalDamage);		// Apply damage to the defender
			}
		}
	}

	/* Knockback - Knocks a unit (defender) away from an attacker's position (attackerTransform) by amount */
	public void Knockback(Transform attackerTransform, GameObject defender, float amount) {
		Vector2 direction = (defender.transform.position - attackerTransform.position).normalized;

		var defenderMoveScript = defender.GetComponent<MoveScript>();
		if(defenderMoveScript) {
			// Some objects might not be able to move
			defenderMoveScript.Push(direction, amount);
		}
	}
}
