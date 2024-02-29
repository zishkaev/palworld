using UnityEngine;

public class Singletone : MonoBehaviour { }

public class Singletone<T> : Singletone where T : Singletone {
	public static T instance { get; private set; }
	protected virtual void Awake() {
		instance = this as T;

		if (transform.parent == null)
			DontDestroyOnLoad(this);
	}
}
