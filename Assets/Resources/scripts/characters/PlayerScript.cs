﻿using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	// Components
	private CharacterScript characterScript;
	private MoveScript moveScript;
	private Animator animator;

	// Animations
	private bool playerAttack = false;

	// Hotkeys
	public char[] abilityKeys;
	
	void Awake() {
		moveScript = GetComponent<MoveScript>();
		characterScript = GetComponent<CharacterScript>();
		animator = GetComponent<Animator>();
	}

	void Start () {
		abilityKeys = new char[2];
		abilityKeys[0] = 'Q';
		abilityKeys[1] = 'W';
	}
	
	// Update is called once per frame
	void Update () {

		/* Player Input */

		// Retrieve axis information from keyboard
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		// Calculate movement per-direction and move the player when a key is pressed
		moveScript.Move(inputX, inputY);

		// Retrieve button information from mouse
		bool attack = Input.GetButtonDown("Fire1");
		attack |= Input.GetButtonDown("Fire2");

		// Watch for ability input
		bool ability1 = Input.GetKeyDown(KeyCode.Q);
		bool ability2 = Input.GetKeyDown(KeyCode.W);	// Todo - figure out how to assign this dynamically

		if(attack) {
			playerAttack = true;	// Used for attack animation
			characterScript.Attack();
		}
		else{
			// Character isn't attacking
			playerAttack = false;
		}

		if(ability1) {
			// Player is executing ability #0
			characterScript.Ability(0);
		}

		if(ability2) {
			// Player is executing ability #1
			characterScript.Ability(1);
		}

		animator.SetBool("playerAttack", playerAttack);

		// Make sure player cannot leave the camera view
//		var dist = (transform.position - Camera.main.transform.position).z;
//
//		var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
//		var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
//
//		var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
//		var bottomBorder = Camera.main.ViewportToWorldPoint (new Vector3(0, 1, dist)).y;
//
//		transform.position = new Vector3(
//			Mathf.Clamp (transform.position.x, leftBorder, rightBorder),
//			Mathf.Clamp (transform.position.y, topBorder, bottomBorder),
//			transform.position.z);

	}
}