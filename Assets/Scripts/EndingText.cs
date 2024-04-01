using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = GameManager.Instance.endingText;
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
