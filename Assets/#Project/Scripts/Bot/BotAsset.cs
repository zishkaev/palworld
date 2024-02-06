using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BotAsset", menuName = "Asset/BotAsset", order = 0)]
public class BotAsset : ScriptableObject {
	public string botName;
	public Sprite icon;
	public GameObject prefab;

	public BotParams botParams;
}

[Serializable]
public struct BotParams {
	public float health;
	public float damage;
	public float size;
	public float speed;
}