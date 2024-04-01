using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Button startGameBtn;
    [SerializeField] Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        startGameBtn.onClick.AddListener(StartGame);
        volumeSlider.onValueChanged.AddListener(ChangeGameVolume);
        volumeSlider.value = GameManager.Instance.volume;
    }

    private void StartGame()
    {
        GameManager.Instance.LoadScene(1);
    }

    private void ChangeGameVolume(float newVolume)
    {
        GameManager.Instance.volume = newVolume;
    }
}
