using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingText : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] DialogLoader dialogLoader;
    [SerializeField] GameObject btns;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.Background != null)
        {
            background.sprite = GameManager.Instance.Background;
        }
        else
        {
            background.gameObject.SetActive(false);
        }

        if (!string.IsNullOrEmpty(GameManager.Instance.endingDialogue))
		{
            dialogLoader.ChangeDialoguePath(GameManager.Instance.endingDialogue);
            dialogLoader.StartNewDialog();

            return;
		}


        btns.SetActive(true);
        GetComponent<TextMeshProUGUI>().text = GameManager.Instance.endingText;

    }

    public void LoadLastLevel()
	{
        GameManager.Instance.LoadLastScene();
	}

    public void Restart()
	{
        GameManager.Instance.ClearInventory();
        GameManager.Instance.LoadScene(0);
	}

    public void Close()
	{
        Application.Quit();
	}
}
