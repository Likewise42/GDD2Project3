using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    public string sceneToLoad;

	public void LoadStoredScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
