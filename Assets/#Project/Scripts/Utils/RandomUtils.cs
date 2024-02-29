using UnityEngine;
using Random = UnityEngine.Random;

public static class RandomUtils {
	public static Vector3 RandomV3 => new Vector3(Random.value, Random.value, Random.value);
	public static Vector3 RandomV3Half => RandomV3 - Vector3.one * 0.5f;
	public static bool Roll(float chance) => Random.value <= chance;
	public static float Range(float min, float max) => Random.Range(min, max);
	public static int Range(int minInclusive, int maxInclusive) => Random.Range(minInclusive, maxInclusive+1);
	public static int RandomIndex(int maxExclusive) => Random.Range(0, maxExclusive);
}
