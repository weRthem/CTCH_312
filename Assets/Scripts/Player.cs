using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float characterSpeed = 10f;

    private Rigidbody2D rb = null;

    private Vector2 direction = new Vector2();
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.position = EntranceManager.Instance.GetSpawnPointByIndex(GameManager.Instance.SpawnPointID);
    }

    // Update is called once per frame
    void Update()
    {
        // Get the inputs for the X and Y axis
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // Get the direction of the inputs and bind it to either -1 or 1
        float velX = Mathf.Sign(inputX);
        float velY = Mathf.Sign(inputY);

        // If the inputs are near 0 set the velocity of that direction to 0 since Mathf.Sign only returns -1 or 1 no 0
        if(inputX > -Mathf.Epsilon && inputX < Mathf.Epsilon)
		{
            velX = 0;
		}

        if (inputY > -Mathf.Epsilon && inputY < Mathf.Epsilon)
        {
            velY = 0;
        }

        direction.Set(velX, velY);

        // Sets the magnitude of the direction that the character is moving to be 1 to keep them from moving faster on an angle
        direction.Normalize();

        rb.velocity = direction * characterSpeed * Time.deltaTime;
    }
}
