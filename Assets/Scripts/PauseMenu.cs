using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
	[SerializeField] Button resumeBtn;
	[SerializeField] Button quitBtn;
    [SerializeField] Slider volumeSlider;

	private void Start()
	{
		volumeSlider.value = GameManager.Instance.volume;
		volumeSlider.onValueChanged.AddListener(UpdateGameVolume);
		resumeBtn.onClick.AddListener(TogglePauseMenu);
		quitBtn.onClick.AddListener(Quit);
	}

	// Update is called once per frame
	void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			TogglePauseMenu();
		}
    }

	private void UpdateGameVolume(float newVolume)
	{
		GameManager.Instance.volume = newVolume;
	}

	public void TogglePauseMenu()
	{
		pauseMenu.SetActive(!pauseMenu.activeInHierarchy);

		if (pauseMenu.activeInHierarchy)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public void Quit()
	{
		Application.Quit();
	}
}
