using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class DialogLoader : MonoBehaviour
{

    [SerializeField] string dialogName = "";
    [SerializeField] bool loadOnStart = false;

    private const string defaultFolder = "GameData/Dialogs";

    private bool isFinishedDialogBox = false;
    private DialogManager dialogManager;

    // Start is called before the first frame update
    void Start()
    {
        dialogManager = DialogManager.Instance;

        dialogManager.StartNewDialogBox.AddListener(StartNewDialogBox);
        dialogManager.ClearText.AddListener(ClearText);
        dialogManager.AppendText.AddListener(AppendText);
        dialogManager.FinishedAddingText.AddListener(FinishedDialogBox);
        dialogManager.EndDialog.AddListener(EndDialog);
        

        if (!loadOnStart) return;

        LoadDialog();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (isFinishedDialogBox)
			{
                DialogManager.Instance.MoveToNextDialogBox();
			}
			else
			{
                isFinishedDialogBox = true;
                DialogManager.Instance.GetRemainingTextInDialog();
			}
		}
    }

    public void LoadDialog()
	{
        string path = Path.Combine(Application.dataPath, defaultFolder, dialogName + ".json");

        DialogManager.Instance.LoadDialog(path);
    }

    private void StartNewDialogBox(Sprite sprite, string characterName)
	{

	}

    private void ClearText()
	{

	}

    private void FinishedDialogBox()
	{

	}

    private void AppendText(string text)
	{

	}

    private void EndDialog()
	{

	}

	private void OnDestroy()
	{
        dialogManager.StartNewDialogBox.RemoveListener(StartNewDialogBox);
        dialogManager.ClearText.RemoveListener(ClearText);
        dialogManager.FinishedAddingText.RemoveListener(FinishedDialogBox);
        dialogManager.AppendText.RemoveListener(AppendText);
        dialogManager.EndDialog.RemoveListener(EndDialog);
    }
}
