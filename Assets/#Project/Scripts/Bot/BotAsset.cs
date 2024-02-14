using UnityEngine;

[CreateAssetMenu(fileName = "BotAsset", menuName = "Asset/BotAsset", order = 0)]
public class BotAsset : ScriptableObject {
	public string botName;
	public Sprite icon;
	public GameObject enemyPrefab, companionPrefab;
}
