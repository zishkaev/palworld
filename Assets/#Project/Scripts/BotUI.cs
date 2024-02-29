using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotUI : MonoBehaviour {
	public TextMeshProUGUI nameText, healthText;
	public Slider healthSlider;
	public GameObject pokeball;

	private BotController controller;

	private void Start() {
		controller = GetComponentInParent<BotController>();
		nameText.text = controller.asset.botName;
		pokeball.SetActive(controller.isPokeball);
		controller.health.onChangeHealth.AddListener(UpdateUI);
		UpdateUI(0);
	}

	private void UpdateUI(float arg0) {
		healthText.text = $"{controller.health.currentHealth}/{controller.health.maxHealth}";
		healthSlider.value = Mathf.Clamp01(controller.health.currentHealth / controller.health.maxHealth);
	}

	private void OnDestroy() {
		controller.health.onChangeHealth.RemoveListener(UpdateUI);
	}
}
