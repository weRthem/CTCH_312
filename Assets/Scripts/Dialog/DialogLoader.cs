using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class DialogLoader : MonoBehaviour
{
    [SerializeField] string dialogName = "";
    [SerializeField] Image dialogueIcon = null;
    [SerializeField] bool playDialogOnStart = false;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button nextDialogBtn;
    [SerializeField] Button[] buttons;
    [SerializeField] Sprite buttonsBackground;
    [SerializeField] Image dialogBackground;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] UnityEvent OnNewDialogueStarted;
    [SerializeField] UnityEvent OnDialogueEnded;

    private const string defaultFolder = "GameData/Dialogs";

    private DialogManager dialogManager;
    private bool isDialogueFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogManager = DialogManager.Instance;
        
        if (!playDialogOnStart) return;

        StartNewDialog();
    }

    public void StartNewDialog()
	{
		if (!dialogManager)
		{
            dialogManager = DialogManager.Instance;
		}

        dialogManager.StartNewDialogBox.AddListener(StartNewDialogBox);
        dialogManager.AppendText.AddListener(AppendText);
        dialogManager.FinishedAddingText.AddListener(FinishedDialogBox);
        dialogManager.EndDialog.AddListener(EndDialog);

        LoadDialog();

        Invoke("StartDialog", 0.25f);
    }

    private void StartDialog()
	{
        dialogManager.MoveToNextDialogBox();
    }

    public void LoadDialog()
	{
        string path = Path.Combine(Application.dataPath, defaultFolder, dialogName + ".json");

        DialogManager.Instance.LoadDialog(path, Path.Combine(Application.dataPath, defaultFolder));
        OnNewDialogueStarted?.Invoke();
    }

    private void StartNewDialogBox(Sprite sprite, string characterName, string[] dialogs)
	{
        if(sprite != null && dialogueIcon != null)
		{
            dialogueIcon.gameObject.SetActive(true);
            dialogueIcon.sprite = sprite;
		}
		else if(dialogueIcon != null)
		{
            dialogueIcon.gameObject.SetActive(false);
		}

        isDialogueFinished = false;
        nextDialogBtn.gameObject.SetActive(false);
        text.text = "";

        if(nameText != null)
		{
            nameText.text = characterName;
		}

        if(dialogs.Length > 1)
		{
			if (dialogBackground)
			{
                dialogBackground.sprite = buttonsBackground;
			}
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
        isDialogueFinished = true;
        nextDialogBtn.gameObject.SetActive(true);
    }

    private void AppendText(string text)
	{
        if (isDialogueFinished) return;
        this.text.text += text;
	}

    private void EndDialog()
	{
        buttons[0].gameObject.SetActive(false);
        buttons[1].gameObject.SetActive(false);

        text.gameObject.SetActive(false);
        nextDialogBtn.gameObject.SetActive(false);
        Deregister();

        OnDialogueEnded?.Invoke();
    }

    public void Deregister()
	{
        dialogManager.StartNewDialogBox.RemoveListener(StartNewDialogBox);
        dialogManager.FinishedAddingText.RemoveListener(FinishedDialogBox);
        dialogManager.AppendText.RemoveListener(AppendText);
        dialogManager.EndDialog.RemoveListener(EndDialog);
    }

    public void ChangeDialoguePath(string newPath)
	{
        dialogName = newPath;
	}

	private void OnDisable()
	{
        Deregister();
	}
}
