using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    [SerializeField] AudioClip onLoadSong = null;
    [SerializeField] AudioClip onUnloadSong = null;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GetComponent<AudioSource>().clip = onLoadSong;
        GameManager.Instance.GetComponent<AudioSource>().Play();
    }

    private void OnDestroy()
    {
        GameManager.Instance.GetComponent<AudioSource>().clip = onUnloadSong;
        GameManager.Instance.GetComponent<AudioSource>().Play();
    }
}
