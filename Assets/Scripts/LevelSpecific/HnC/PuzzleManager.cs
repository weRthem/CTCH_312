using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
	public RectTransform[] pieceSlots;
	public UnityEvent OnPuzzleCompleted;
	public UnityEvent OnPuzzleClosed;

	private Player player = null;

	private PuzzlePiece[] pieces;
	private Item[] items;

	private void Awake()
	{
		player = Player.Instance;
		items = player.GetItems();
		pieces = GetComponentsInChildren<PuzzlePiece>();
	}

	private void OnEnable()
	{
		for (int i = 0; i < pieces.Length; i++)
		{
			pieces[i].gameObject.SetActive(DoesPlayerHavePuzzlePiece(pieces[i], items));
		}
	}

	private bool DoesPlayerHavePuzzlePiece(in PuzzlePiece piece, in Item[] items)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if(int.TryParse(items[i].itemName, out int number))
			{
				Debug.Log(number);
				if (piece.pieceNumber == number) return true;
			}
		}

		return false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			OnPuzzleClosed?.Invoke();
		}
	}

	public void CheckIfAllPicesAreInCorrectSpots()
	{
		for (int i = 0; i < pieces.Length; i++)
		{
			if (!pieces[i].IsInCorrectSpot) return;
		}

		for (int i = 0; i < pieces.Length; i++)
		{
			player.RemoveItem(pieces[i].pieceNumber.ToString());
		}

		OnPuzzleClosed?.Invoke();
		OnPuzzleCompleted?.Invoke();
	}
}
