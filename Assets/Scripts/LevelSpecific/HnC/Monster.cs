using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster : MonoBehaviour
{
    [SerializeField] float timeBetweenAttacks = 60f;
    [SerializeField] float warningTime = 5f;
    [SerializeField] float speed = 0.5f;
    [SerializeField] Vector2 targetPosition;
    [SerializeField] SpriteRenderer playerSpriteRenderer = null;
    [SerializeField] GameObject exclemationPoint = null;

    private bool paused = false;

    private float currentTime = 0f;
    private bool attacking = false;
    private float lerpProgress = 1f;
    private Vector2 startPos;
    private bool spottedPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (paused) return;

		if (attacking)
		{
			if (playerSpriteRenderer.enabled)
			{
                spottedPlayer = true;
			}

            lerpProgress += Time.deltaTime * speed;

            lerpProgress = Mathf.Clamp01(lerpProgress);
            Vector2 pos1 = Vector2.Lerp(startPos, targetPosition, lerpProgress);

            transform.position = pos1;

            if(lerpProgress >= 1)
			{
                attacking = false;
                lerpProgress = 0f;

                if (!spottedPlayer) return;

                GameManager.Instance.endingText = "The entity spotted you. Hide next time";
                SceneManager.LoadScene(6);
			}

            return;
		}

        lerpProgress += Time.deltaTime * speed;

        lerpProgress = Mathf.Clamp01(lerpProgress);
        Vector2 pos = Vector2.Lerp(targetPosition, startPos, lerpProgress);

        transform.position = pos;

        currentTime += Time.deltaTime;

        if(currentTime >= timeBetweenAttacks)
		{
            attacking = true;
            lerpProgress = 0f;
            currentTime = 0f;
            exclemationPoint.gameObject.SetActive(false);
            currentTime = 0f;
        }
        else if (currentTime >= timeBetweenAttacks - warningTime && !exclemationPoint.gameObject.activeInHierarchy)
		{
            exclemationPoint.gameObject.SetActive(true);
		}
    }

    public void SetPause(bool isPaused)
	{
        paused = isPaused;
	}
}
