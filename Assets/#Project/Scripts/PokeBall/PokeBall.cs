using UnityEngine;

public class PokeBall : MonoBehaviour
{
    public BotAsset asset;
	public GameObject takeUI;
    public TriggerVolume triggerVolume;

	private bool showed;

	private void Start() {
		triggerVolume.Enter += Enter;
		triggerVolume.Exit += Exit;
	}

	private void Enter() {
		ShowUI(true);
	}

	private void Exit() {
		ShowUI(false);
	}

	private void ShowUI(bool state) {
		showed = state;
		takeUI.SetActive(state);
	}

	private void Update() {
		if (!showed) return;
		if (Input.GetKeyDown(KeyCode.E)) {
			if (PokeBallController.instance.TakePokeBall(this)) {
				Destroy(gameObject);
			}
		}
	}
}
