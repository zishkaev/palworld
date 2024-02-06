using Invector.vCharacterController.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BotController : MonoBehaviour {
	public BotAsset asset;
	public vSimpleMeleeAI_Controller controller;
	public bool isPokeball;
	private bool createdPokeBall;

	private void Start() {
		controller.onChangeHealth.AddListener(ChangeHealth);
	}

	private void ChangeHealth(float value) {
		if (value <= 0) {
			if (isPokeball && !createdPokeBall) {
				createdPokeBall = true;
				PokeBallController.instance.CreatePokeBall(asset, transform.position);
				gameObject.SetActive(false);
			}
		}
	}
}
