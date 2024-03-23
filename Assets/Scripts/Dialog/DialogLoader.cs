using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class DialogLoader : MonoBehaviour
{

    [SerializeField] string dialogName = "";
    [SerializeField] bool playDialogOnStart = false;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button nextDialogBtn;
    [SerializeField] Button[] buttons;

    private const string defaultFolder = "GameData/Dialogs";

    private bool isFinishedDialogBox = false;
    private DialogManager dialogManager;

    // Start is called before the first frame update
    void Start()
    {
        dialogManager = DialogManager.Instance;

        dialogManager.StartNewDialogBox.AddListener(StartNewDialogBox);
        dialogManager.AppendText.AddListener(AppendText);
        dialogManager.FinishedAddingText.AddListener(FinishedDialogBox);
        dialogManager.EndDialog.AddListener(EndDialog);
        
        LoadDialog();

        if (!playDialogOnStart) return;

        Invoke("StartDialog", 0.02f);
    }

    private void StartDialog()
	{
        dialogManager.MoveToNextDialogBox();
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
                DialogManager.Instance.GetRemainingTextInDialog();
			}
		}
    }

    public void LoadDialog()
	{
        string path = Path.Combine(Application.dataPath, defaultFolder, dialogName + ".json");

        DialogManager.Instance.LoadDialog(path);
    }

    private void StartNewDialogBox(Sprite sprite, string characterName, string[] dialogs)
	{
        isFinishedDialogBox = false;
        nextDialogBtn.gameObject.SetActive(false);
        text.text = "";

        if(dialogs.Length > 1)
		{
            text.gameObject.SetActive(false);
            buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = dialogs[0];
            buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = dialogs[1];

            buttons[0].gameObject.SetActive(true);
            buttons[1].gameObject.SetActive(true);
            return;
		}

        text.gameObject.SetActive(true);
        buttons[0].gameObject.SetActive(false);
        buttons[1].gameObject.SetActive(false);
    }

    private void FinishedDialogBox()
	{
        isFinishedDialogBox = true;
        nextDialogBtn.gameObject.SetActive(true);
    }

    private void AppendText(string text)
	{
        if (isFinishedDialogBox) return;
        this.text.text += text;
	}

    private void EndDialog()
	{
        buttons[0].gameObject.SetActive(false);
        buttons[1].gameObject.SetActive(false);

        text.gameObject.SetActive(false);
        nextDialogBtn.gameObject.SetActive(false);
    }

	private void OnDestroy()
	{
        dialogManager.StartNewDialogBox.RemoveListener(StartNewDialogBox);
        dialogManager.FinishedAddingText.RemoveListener(FinishedDialogBox);
        dialogManager.AppendText.RemoveListener(AppendText);
        dialogManager.EndDialog.RemoveListener(EndDialog);
    }
}
