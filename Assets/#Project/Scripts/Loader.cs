using UnityEngine;

public class Loader : MonoBehaviour {
	public SceneType scene;

	public void Load() {
		SceneController.instance.LoadScene(scene);
	}
}
