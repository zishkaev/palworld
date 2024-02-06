using Invector.vCharacterController;
using UnityEngine;

public class SpawnController : Singletone<SpawnController> {
	private Transform playerTransform;


	private void Start() {
		var cont = FindObjectOfType<vThirdPersonController>();
		if (cont != null) {
			playerTransform = cont.transform;
		}
	}

	public void Spawn(BotAsset asset) {
		
	}

	public void SpawnNearPlayer(BotAsset asset) {
		var a = UnityEngine.Random.Range(0, 360);
		Vector3 p = playerTransform.position + Quaternion.Euler(0,a,0)* Vector3.forward;
		Spawn(asset.prefab, p, Quaternion.identity);
	}

	public void Spawn(GameObject prefab, Vector3 p, Quaternion q) {
		Instantiate(prefab, p, q);
	}
}
