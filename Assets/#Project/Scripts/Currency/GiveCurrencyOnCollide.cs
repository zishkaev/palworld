using UnityEngine;

public sealed class GiveCurrencyOnCollide : MonoBehaviour {
	public int _amount;
	[SerializeField]
	float _radius = 0.1f;

	public void Update() {
		if (Vector3.Distance(transform.position, PlayerController.instance.mainPoint.position) < _radius) {
			CurrencySystem.instance.Add(_amount);
			Destroy(gameObject);
		}
	}
}
