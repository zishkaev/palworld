using System;
using UnityEngine;

public class PokeBallController : Singletone<PokeBallController> {
	public PokeBall pokeballPrefab;
	public PokeItem[] items;
	public Action OnUpdatePokeball;

	public void Update() {
		for (int i = 0; i < items.Length; i++) {
			if (Input.GetKeyDown(items[i].keyCode)) {
				if (items[i].asset != null) {
					SpawnController.instance.SpawnNearPlayer(items[i].asset);
					items[i].asset = null;
					OnUpdatePokeball?.Invoke();
				}
			}
		}
	}

	public void CreatePokeBall(BotAsset asset, Vector3 point) {
		var p = Instantiate(pokeballPrefab, point, Quaternion.identity);
		p.asset = asset;
	}

	public bool TakePokeBall(PokeBall pokeBall) {
		for (int i = 0; i < items.Length; i++) {
			if (items[i].asset == null) {
				items[i].asset = pokeBall.asset;
				OnUpdatePokeball?.Invoke();
				return true;
			}
		}
		return false;
	}
}

[Serializable]
public struct PokeItem {
	public BotAsset asset;
	public KeyCode keyCode;
}
