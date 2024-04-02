using UnityEngine;

[System.Serializable]
public class Dialog
{
	public float TimeBetweenLetters;
	public DialogText[] Texts;
	public int currentIndex = 0;
}

[System.Serializable]
public class DialogText
{
	public string IconPath;
	public string characterName;
	public string[] text;
	public int currentTextPosition = 0;
	public ResourceRequest iconLoader = null;

	public void OnIconLoaded(AsyncOperation operation)
	{
		if(iconLoader == null)
		{
			Debug.LogError("No icon loader was provided");
			return;
		}

		if (!iconLoader.isDone) return;

		DialogManager.Instance.AddSpriteToDialogueSprites(IconPath, (Texture2D)iconLoader.asset);
		iconLoader = null;
	}
}