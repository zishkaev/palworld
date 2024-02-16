using UnityEngine;
using UnityEngine.AI;

public class SpawnZone : MonoBehaviour {
	public Vector3 size;

	public Vector3 GetSpawnPoint() {
		float x = Random.Range(-size.x, size.x);
		float z = Random.Range(-size.z, size.z);
		float y = -size.y;
		Vector3 spawnPoint = transform.position + new Vector3(x, y, z);
		if (FindClosestPointOnNavmesh(ref spawnPoint))
			return spawnPoint;
		else
			return transform.position;
	}

	public bool FindClosestPointOnNavmesh(ref Vector3 _point, float dist = 1f) {
		if (dist > 10f) return false; //10 tries, may be change to config in future
		if (NavMesh.SamplePosition(_point, out NavMeshHit hit, dist, 1)) { //1 is nav mesh layer
			_point = hit.position;
			return true;
		}
		return FindClosestPointOnNavmesh(ref _point, dist + 1f);
	}

	private void OnValidate() {
		size = transform.localScale / 2;
	}
}
