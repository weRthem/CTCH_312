using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] int sceneToLoad = 0;

    public void LoadScene()
	{
		GameManager.Instance.LoadScene(sceneToLoad);
	}

	public void LoadLastScene()
	{
		GameManager.Instance.LoadLastScene();
	}

	public void SetSceneToLoad(int scene)
	{
		sceneToLoad = scene;
	}
}
