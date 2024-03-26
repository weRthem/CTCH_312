using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemUser : MonoBehaviour
{
    [SerializeField] Item item;
	[SerializeField] UnityEvent OnCorrectItemUsed;
	[SerializeField] UnityEvent OnIncorrectItemUsed;

    public void AddItemToPlayer()
	{
		Player.Instance.AddItem(item);
	}

	public void RemoveItemFromPlayer()
	{
		Player.Instance.RemoveItem(item.itemName);
	}

	public void CheckIfPlayerIsUsingRequiredItem()
	{
		Item playersCurrentItem = Player.Instance.GetCurrentlySelectedItem();

		if (playersCurrentItem.itemName != item.itemName)
		{
			OnIncorrectItemUsed?.Invoke();
			return;
		}

		OnCorrectItemUsed?.Invoke();
	}
}
