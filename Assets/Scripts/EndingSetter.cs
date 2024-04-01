using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSetter : MonoBehaviour
{
    [SerializeField] Sprite endingSprite;
    [SerializeField] string endingText;
    [SerializeField] string endingDialogue;

    public void SetEnding()
    {
        GameManager.Instance.endingText = endingText;
        GameManager.Instance.Background = endingSprite;
        GameManager.Instance.endingDialogue = endingDialogue;
    }
}
