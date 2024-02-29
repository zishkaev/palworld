using UnityEngine.SceneManagement;

public class SceneController : Singletone<SceneController> {
	public void LoadScene(SceneType sceneType) {
		SceneManager.LoadScene((int)sceneType);
	}
}

public enum SceneType {
	Menu = 0,
	Game = 1
}