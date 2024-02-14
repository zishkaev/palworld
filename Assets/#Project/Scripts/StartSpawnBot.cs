using Invector;
using UnityEngine;

public class StartSpawnBot : MonoBehaviour {
	public BotAsset[] botAssets;

	public int botCount;

	public Vector3 spawnBorderSize;
	public Vector2 sizeBorder;

	public void Start() {
		SpawnBots();
	}

	public void SpawnBots() {
		for (int i = 0; i < botCount; i++) {
			SpawnBot();
		}
	}

	public void SpawnBot() {
		var asset = botAssets[Random.Range(0, botAssets.Length)];
		float size = Random.Range(sizeBorder.x, sizeBorder.y);

		float x = Random.Range(spawnBorderSize.x, spawnBorderSize.z);
		float z = Random.Range(spawnBorderSize.x, spawnBorderSize.z);
		Vector3 p = transform.position + new Vector3(x, 0, z);

		RaycastHit hit;
		if (Physics.Raycast(p, Vector3.down, out hit, spawnBorderSize.y)) {
			p.y = hit.point.y;
		}

		var bot = SpawnController.instance.Spawn(asset.enemyPrefab, p, Quaternion.identity);
		bot.transform.localScale = Vector3.one * size;
		var health = bot.GetComponentInChildren<vHealthController>();
		if (health != null) {
			health.maxHealth = (int)(size * 100);
			health.ResetHealth();
		}
	}
}
