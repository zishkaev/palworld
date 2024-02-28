using System.Collections;
using UnityEngine;

public class Loader : MonoBehaviour {
	public SceneType scene;
    [SerializeField] LevelFadingScreen levelFadingScreen;


    void Awake()
    {

        //levelFadingScreen = new LevelFadingScreen();


    }


	public void Load() {
        
        levelFadingScreen.FadeToLevel();
        // SceneController.instance.LoadScene(scene);
        Invoke(nameof(LoadScene), 3);
    }

    

    public void LoadScene() 
    {
        SceneController.instance.LoadScene(scene);
    }
}
