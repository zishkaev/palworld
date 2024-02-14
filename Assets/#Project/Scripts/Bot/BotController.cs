using Invector;
using UnityEngine;

public class BotController : MonoBehaviour {
	public BotAsset asset;
	public vHealthController controller;
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
