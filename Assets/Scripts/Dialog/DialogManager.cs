using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    public UnityEvent<Sprite, string, string[]> StartNewDialogBox;
    public UnityEvent<string> AppendText;
    public UnityEvent FinishedAddingText;
    public UnityEvent EndDialog;

    private bool isDialogPlaying = false;

    private Dialog currentDialog = null;
    private float timeSinceLastCharacter = 0f;
    private Dictionary<string, Sprite> dialogueSprites = new Dictionary<string, Sprite>();
    private bool isFinishedDialogue = false;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

/*        Directory.CreateDirectory(Application.dataPath + "/GameData/Dialogs/");

        FileStream fs = File.Create(Application.dataPath + "/GameData/Dialogs/TempDialog.json");

        Dialog tempDialog = new Dialog();

        tempDialog.Texts = new DialogText[1];
        tempDialog.Texts[0] = new DialogText();

        tempDialog.Texts[0].text = new string[1];
        tempDialog.Texts[0].characterName = "name";
        tempDialog.Texts[0].IconPath = "path";
        tempDialog.Texts[0].text[0] = "asdf";

        string json = JsonUtility.ToJson(tempDialog);

        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);

        fs.Write(bytes);*/
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDialog == null) return;

        if (Input.GetKeyDown(KeyCode.Space) && currentDialog.Texts[currentDialog.currentIndex].text.Length == 1)
        {
            if (isFinishedDialogue)
            {
                MoveToNextDialogBox();
            }
            else
            {
                GetRemainingTextInDialog();
            }
        }

        if (!isDialogPlaying) return;
        if (currentDialog.Texts[currentDialog.currentIndex].text.Length > 1) return;

        timeSinceLastCharacter += Time.deltaTime;

        if (timeSinceLastCharacter < currentDialog.TimeBetweenLetters) return;

        DialogText dialogText = currentDialog.Texts[currentDialog.currentIndex];

        if (dialogText.currentTextPosition >= dialogText.text[0].Length) return;

        AppendText?.Invoke(dialogText.text[0][dialogText.currentTextPosition].ToString());

        dialogText.currentTextPosition++;

        if (dialogText.currentTextPosition >= dialogText.text[0].Length)
		{
            isFinishedDialogue = true;
            FinishedAddingText?.Invoke();
		}

        timeSinceLastCharacter = 0f;
    }

    public async void LoadDialog(string path)
	{
        if (!File.Exists(path))
		{
            Debug.LogError("Could not find file at: " + path);
            return;
		}

        string json = await File.ReadAllTextAsync(path);

        currentDialog = JsonUtility.FromJson<Dialog>(json);

		for (int i = 0; i < currentDialog.Texts.Length; i++)
		{
            DialogText dialogText = currentDialog.Texts[i];

            if (dialogText == null) continue;

            if (dialogueSprites.ContainsKey(dialogText.IconPath)) continue;

            dialogText.iconLoader = Resources.LoadAsync<Sprite>(dialogText.IconPath);

            dialogText.iconLoader.completed += dialogText.OnIconLoaded;

            dialogueSprites.Add(dialogText.IconPath, null);
        }

        return;
	}

    public void MoveToNextDialogBox()
	{
        if (currentDialog == null) return;
        isFinishedDialogue = false;

        if (!isDialogPlaying)
		{
            isDialogPlaying = true;

            EnableNewDialogBox();

            return;
		}

        currentDialog.currentIndex++;

        EnableNewDialogBox();
	}

    private void EnableNewDialogBox()
	{
        if (currentDialog == null) return;

        if (currentDialog.currentIndex >= currentDialog.Texts.Length)
        {
            EndDialog?.Invoke();
            currentDialog = null;
            isDialogPlaying = false;
            return;
        }

        DialogText dialogText = currentDialog.Texts[currentDialog.currentIndex];

        Sprite dialogSprite = dialogueSprites[dialogText.IconPath];

        StartNewDialogBox?.Invoke(dialogSprite, dialogText.characterName, dialogText.text);
    }

    public void GetRemainingTextInDialog()
	{
        DialogText currentText = currentDialog.Texts[currentDialog.currentIndex];
        string remainingDialog = "";

        for (int i = currentText.currentTextPosition; i < currentText.text[0].Length; i++)
		{
            remainingDialog += currentText.text[0][i];
            currentText.currentTextPosition = i;
		}

        AppendText?.Invoke(remainingDialog);
        isFinishedDialogue = true;
        FinishedAddingText?.Invoke();
	}

    public void AddSpriteToDialogueSprites(string key, Sprite sprite)
	{
        if (!dialogueSprites.ContainsKey(key)) return;

        dialogueSprites[key] = sprite;
	}
}
