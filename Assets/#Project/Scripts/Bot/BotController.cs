using Invector;
using UnityEngine;

public class BotController : MonoBehaviour {
	public CurrencyLoot loot;
	public BotAsset asset;
	public vHealthController health;
	public bool isPokeball;

	private bool dead;
	private float delayDestroy = 5f;

	private void Start() {
		health.onChangeHealth.AddListener(ChangeHealth);
	}

	private void ChangeHealth(float value) {
		if (value <= 0 && !dead) {
			dead = true;
			if (isPokeball) {
				PokeBallController.instance.CreatePokeBall(asset, transform.position);
				gameObject.SetActive(false);
			} 
			loot.SpawnLoot(transform.position);
			Destroy(gameObject, delayDestroy);
		}
	}
}
