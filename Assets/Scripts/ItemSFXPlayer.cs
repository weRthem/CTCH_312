using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSFXPlayer : MonoBehaviour
{
	private void OnEnable()
	{
		GetComponent<AudioSource>().volume = GameManager.Instance.volume;
		GetComponent<AudioSource>().Play();
	}
}
