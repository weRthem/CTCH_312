using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceManager : MonoBehaviour
{
	public static EntranceManager Instance { get; private set; }

    [SerializeField] Vector2[] spawnPoints;

	private void Awake()
	{
		Instance = this;
	}

	public Vector2 GetSpawnPointByIndex(int index)
	{
		if (index < 0 || index >= spawnPoints.Length) return Vector2.zero;

		return spawnPoints[index];
	}
}
