using Invector;
using UnityEngine;

public class BotController : MonoBehaviour {
	public BotAsset asset;
	public vHealthController health;
	public bool isPokeball;

	private bool createdPokeBall;
	private float delayDestroy = 5f;

	private void Start() {
		health.onChangeHealth.AddListener(ChangeHealth);
	}

	private void ChangeHealth(float value) {
		if (value <= 0) {
			if (isPokeball && !createdPokeBall) {
				createdPokeBall = true;
				PokeBallController.instance.CreatePokeBall(asset, transform.position);
				gameObject.SetActive(false);
			}
			Destroy(gameObject, delayDestroy);
		}
	}
}
