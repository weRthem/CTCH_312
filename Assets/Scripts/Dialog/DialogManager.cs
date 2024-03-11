using UnityEngine;
using UnityEngine.Events;
using System.IO;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    public UnityEvent<Sprite, string> StartNewDialogBox;
    public UnityEvent<string> AppendText;
    public UnityEvent ClearText;
    public UnityEvent FinishedAddingText;
    public UnityEvent EndDialog;

    private Dialog currentDialog = null;
    private float timeSinceLastCharacter = 0f;
    private Dictionary<string, Sprite> dialogueSprites = new Dictionary<string, Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        Directory.CreateDirectory(Application.dataPath + "/GameData/Dialogs/");

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

        fs.Write(bytes);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDialog == null) return;

        timeSinceLastCharacter += Time.deltaTime;

        if (timeSinceLastCharacter < currentDialog.TimeBetweenLetters) return;

        DialogText dialogText = currentDialog.Texts[currentDialog.currentIndex];

        if (dialogText.currentTextPosition >= dialogText.text.Length) return;

        AppendText?.Invoke(dialogText.text[dialogText.currentTextPosition]);

        dialogText.currentTextPosition++;

        if (dialogText.currentTextPosition >= dialogText.text.Length)
		{
            FinishedAddingText?.Invoke();
		}
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
        ClearText?.Invoke();
        currentDialog.currentIndex++;

        if(currentDialog.currentIndex >= currentDialog.Texts.Length)
		{
            EndDialog?.Invoke();
            currentDialog = null;
            return;
		}

        DialogText dialogText = currentDialog.Texts[currentDialog.currentIndex];

        Sprite dialogSprite = dialogueSprites[dialogText.IconPath];

        StartNewDialogBox?.Invoke(dialogSprite, dialogText.characterName);
	}

    public void GetRemainingTextInDialog()
	{
        DialogText currentText = currentDialog.Texts[currentDialog.currentIndex];
        string remainingDialog = "";

        for (int i = currentText.currentTextPosition; i < currentText.text.Length; i++)
		{
            remainingDialog += currentText.text[i];
		}

        AppendText?.Invoke(remainingDialog);
	}

    public void AddSpriteToDialogueSprites(string key, Sprite sprite)
	{
        if (!dialogueSprites.ContainsKey(key)) return;

        dialogueSprites[key] = sprite;
	}
}
