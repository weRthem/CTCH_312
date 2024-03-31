using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

    public int SpawnPointID { get; set; } = 0;
	public Item[] items = new Item[10];
	public Sprite Background;
	public string endingText;
	private int lastSceneIndex = 0;

	private void Awake()
	{
		if (!Instance)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
			return;
		}
	}

	public void LoadScene(int sceneIndex)
	{
		lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(sceneIndex);
	}

	public void LoadLastScene()
	{
		SceneManager.LoadScene(lastSceneIndex);
	}
}
