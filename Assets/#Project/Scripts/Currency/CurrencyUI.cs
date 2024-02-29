using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour {
	public TextMeshProUGUI textUI;

	private void Start() {
		CurrencySystem.instance.OnChangeValue += UpdateValue;
		UpdateValue();
	}

	private void UpdateValue() {
		textUI.text = CurrencySystem.instance.CurrentValue.ToString();
	}

	private void OnDestroy() {
		CurrencySystem.instance.OnChangeValue -= UpdateValue;
	}
}
