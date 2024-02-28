using Invector.vCharacterController;
using Invector.vCharacterController.AI;
using System.Collections;
using UnityEngine;

public class SpawnController : Singletone<SpawnController> {
	public void Spawn(BotAsset asset) {
		
	}

	public void SpawnNearPlayer(BotAsset asset) {
		var a = Random.Range(0, 360);
		Vector3 p = PlayerController.instance.transform.position + Quaternion.Euler(0,a,0)* Vector3.forward;
		var bot = Spawn(asset.companionPrefab, p, Quaternion.identity);
		StartCoroutine(SetSettingsForCompanion(bot));
	}

	IEnumerator SetSettingsForCompanion(GameObject bot) {
		var companion = bot.GetComponentInChildren<vSimpleMeleeAI_Companion>();
		if (companion) {
			companion.companion = PlayerController.instance.transform;
			yield return null;
			companion.companionState = vSimpleMeleeAI_Companion.CompanionState.Follow;
		}
	}

	public GameObject Spawn(GameObject prefab, Vector3 p, Quaternion q) {
		return Instantiate(prefab, p, q);
	}
}
