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
	public string endingDialogue;
	public float volume = 0.5f;
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

	private void Update()
	{
		GetComponent<AudioSource>().volume = volume;
	}

	public void LoadScene(int sceneIndex)
	{
		lastSceneIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(sceneIndex);
	}

	public void LoadLastScene()
	{
		Background = null;
		endingText = ""; 
		SceneManager.LoadScene(lastSceneIndex);
	}

	public void ClearInventory()
	{
		items = new Item[10];
	}
}
