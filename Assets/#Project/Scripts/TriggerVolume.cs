using Invector.vCharacterController;
using System;
using UnityEngine;

public class TriggerVolume : MonoBehaviour {
	public Action Enter, Exit;

	private void OnTriggerEnter(Collider other) {
		var player = other.GetComponentInParent<vThirdPersonController>();
		if (player != null)
			Enter?.Invoke();
	}

	private void OnTriggerExit(Collider other) {
		var player = other.GetComponentInParent<vThirdPersonController>();
		if (player != null)
			Exit?.Invoke();
	}
}
