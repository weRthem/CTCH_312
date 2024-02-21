using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    [SerializeField] int sceneToLoad = 0;
    [SerializeField] int spawnPointIndex = -1;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.GetComponentInParent<Player>()) return;

		GameManager.Instance.SpawnPointID = spawnPointIndex;
		SceneManager.LoadScene(sceneToLoad);
	}
}
