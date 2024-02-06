using System;
using UnityEngine;
using UnityEngine.UI;

public class PokeBallUI : MonoBehaviour {
	public Image[] images;

	private void Start() {
		PokeBallController.instance.OnUpdatePokeball += UpdateUI;
	}

	public void UpdateUI() {
		for(int i=0;i<PokeBallController.instance.items.Length;i++) {
			if (images.Length > i) {
				images[i].sprite = PokeBallController.instance.items[i].asset ? PokeBallController.instance.items[i].asset.icon : null;
			}
		}
	}

	private void OnDestroy() {
		PokeBallController.instance.OnUpdatePokeball -= UpdateUI;
	}
}
