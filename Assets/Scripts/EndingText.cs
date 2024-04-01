using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingText : MonoBehaviour
{
    [SerializeField] Image background;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = GameManager.Instance.endingText;

        if(GameManager.Instance.Background != null)
        {
            background.sprite = GameManager.Instance.Background;
        }
        else
        {
            background.gameObject.SetActive(false);
        }
    }

    public void LoadLastLevel()
	{
        GameManager.Instance.LoadLastScene();
	}

    public void Restart()
	{
        GameManager.Instance.LoadScene(0);
	}

    public void Close()
	{
        Application.Quit();
	}
}
