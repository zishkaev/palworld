using UnityEngine;

public class StartSpawnBot : MonoBehaviour {
	public BotAsset[] botAssets;
	public int botCount;

	//public Vector3 spawnBorderSize;
	//public Vector2 sizeBorder;

	private SpawnZone[] spawnZones;

	public void Start() {
		spawnZones = GetComponentsInChildren<SpawnZone>();
		SpawnBots();
	}

	public void SpawnBots() {
		for (int i = 0; i < botCount; i++) {
			SpawnBot();
		}
	}

	public void SpawnBot() {
		var asset = botAssets[Random.Range(0, botAssets.Length)];
		//float size = Random.Range(sizeBorder.x, sizeBorder.y);
		//
		//float x = Random.Range(spawnBorderSize.x, spawnBorderSize.z);
		//float z = Random.Range(spawnBorderSize.x, spawnBorderSize.z);
		//Vector3 p = transform.position + new Vector3(x, 0, z);

		//RaycastHit hit;
		//if (Physics.Raycast(p, Vector3.down, out hit, spawnBorderSize.y)) {
		//	p.y = hit.point.y;
		//}
		
		Vector3 p = spawnZones[Random.Range(0, spawnZones.Length)].GetSpawnPoint();
		var bot = SpawnController.instance.Spawn(asset.enemyPrefab, p, Quaternion.identity);
		SettingBot(bot);
		//bot.transform.localScale = Vector3.one * size;
	}

	private void SettingBot(GameObject bot) {
		var controller = bot.GetComponent<BotController>();
		int size = Random.Range(1, botAssets.Length);
		controller.health.maxHealth = (int)(size * 100);
		controller.health.ResetHealth();
		controller.isPokeball = Random.value < 0.5f;
	}
}
