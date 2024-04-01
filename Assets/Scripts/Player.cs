using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] float characterSpeed = 10f;
    [SerializeField] TextMeshProUGUI interactionText = null;
    [SerializeField] ItemVisual[] itemVisuals;
    [SerializeField] SpriteRenderer bodyRenderer = null;

    public bool Paused { get; private set; } = false;

    private Rigidbody2D rb = null;

    private Vector2 direction = new Vector2();
    private Interaction currentlySelectedInteration = null;
    private Item[] items = new Item[10];
    private int currentHotBarNumber = 1;
    private float velX;
    private float velY;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();

        rb.position = EntranceManager.Instance.GetSpawnPointByIndex(GameManager.Instance.SpawnPointID);

        for (int i = 0; i < GameManager.Instance.items.Length; i++)
        {
            items[i] = GameManager.Instance.items[i];
        }

        UpdateItemVisuals();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the inputs for the X and Y axis
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // Get the direction of the inputs and bind it to either -1 or 1
        velX = Mathf.Sign(inputX);
        velY = Mathf.Sign(inputY);


        // If the inputs are near 0 set the velocity of that direction to 0 since Mathf.Sign only returns -1 or 1 no 0
        if(inputX > -Mathf.Epsilon && inputX < Mathf.Epsilon)
		{
            velX = 0;
		}

        if (inputY > -Mathf.Epsilon && inputY < Mathf.Epsilon)
        {
            velY = 0;
        }

        if (velY == 0f && velX == 0f)
        {
            GetComponent<Animator>().Play("Idle");
        }
        else if (velX < 0)
		{
            GetComponent<Animator>().Play("Walk");
            bodyRenderer.flipX = false;
        }
		else if(velX > 0)
		{
            GetComponent<Animator>().Play("Walk");
            bodyRenderer.flipX = true;
        }
		else
		{
            GetComponent<Animator>().Play("Walk");
        }

		if (currentlySelectedInteration && Input.GetKeyDown(KeyCode.E))
		{
            currentlySelectedInteration.Interacted?.Invoke();
		}

		for (int i = 0; i < 10; i++)
		{
			if (Input.GetKeyDown((KeyCode)(i + 48)))
			{
                // 48 is the enum value for 0 and 57 is the enum value for 9
                currentHotBarNumber = i;
                UpdateItemVisuals();
			}
		}
    }

	private void FixedUpdate()
	{
        if (Paused) return;

        direction.Set(velX, velY);

        // Sets the magnitude of the direction that the character is moving to be 1 to keep them from moving faster on an angle
        direction.Normalize();

        rb.velocity = (direction * characterSpeed) * Time.fixedDeltaTime;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.TryGetComponent(out Interaction interaction))
		{
            interactionText.text = interaction.interactText;

            currentlySelectedInteration = interaction;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
        if (collision.TryGetComponent(out Interaction interaction))
        {
            interactionText.text = "";

            currentlySelectedInteration = null;
        }
    }

    private void UpdateItemVisuals()
	{
        if(itemVisuals.Length != items.Length)
		{
            Debug.LogError("item visuals and items aren't the same length");
            return;
		}

		for (int i = 0; i < itemVisuals.Length; i++)
		{
            if(items[i].itemIcon == null)
			{
                itemVisuals[i].icon.color = Color.clear;
            }
			else
			{
                itemVisuals[i].icon.sprite = items[i].itemIcon;
                itemVisuals[i].icon.color = Color.white;
            }
            
            if(i == currentHotBarNumber)
			{
                Color grey = Color.grey;

                grey.a = 0.5f;
                itemVisuals[i].background.color = grey;
			}
			else
			{
                Color black = Color.black;

                black.a = 0.5f;
                itemVisuals[i].background.color = black;
            }
		}
	}

    public void AddItem(Item item)
	{
		for (int i = 1; i < items.Length; i++)
		{
            Debug.Log("Checking item: " + i);
            if(string.IsNullOrEmpty(items[i].itemName))
			{
                items[i] = item;
                UpdateItemVisuals();
                return;
			}
		}

		if (string.IsNullOrEmpty(items[0].itemName))
		{
            items[0] = item;
            UpdateItemVisuals();
        }
	}

    public Item GetCurrentlySelectedItem()
	{
        if (currentHotBarNumber < 0 || currentHotBarNumber > 9) return new Item();

        return items[currentHotBarNumber];
	}

    public void RemoveItem(string itemName)
	{
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == itemName)
            {
                items[i] = new Item();
                UpdateItemVisuals();
                return;
            }
        }
    }

    public void SendItemsToGameManager()
    {
        for (int i = 0; i < items.Length; i++)
        {
            GameManager.Instance.items[i] = items[i];
        }
    }

    public void SetPaused(bool isPaused)
	{
        Paused = isPaused;
	}

    public Item[] GetItems()
	{
        return items;
	}
}

[System.Serializable]
public struct Item
{
    public string itemName;
    public Sprite itemIcon;
}

[System.Serializable]
public struct ItemVisual
{
    public UnityEngine.UI.Image background;
    public UnityEngine.UI.Image icon;
}