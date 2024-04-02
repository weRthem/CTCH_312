using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePiece : MonoBehaviour
{
    [SerializeField] private float snappingDistance = 25f;
    public int pieceNumber = 0;
    public bool IsInCorrectSpot { get; private set; } = false;

    private PuzzleManager puzzleManager = null;
    private int UILayer;
    private bool isFollowingMouse;

    // Start is called before the first frame update
    void Start()
    {
        puzzleManager = GetComponentInParent<PuzzleManager>();
        UILayer = LayerMask.NameToLayer("UI");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInCorrectSpot) return;

		if (Input.GetMouseButtonDown(0) && IsPointerOverThisUIElement())
		{
            isFollowingMouse = true;
            transform.rotation = Quaternion.identity;
		}

        if (!isFollowingMouse) return;

		if (Input.GetMouseButtonUp(0))
		{
            isFollowingMouse = false;

			for (int i = 0; i < puzzleManager.pieceSlots.Length; i++)
			{
				if (Vector2.Distance(transform.position, puzzleManager.pieceSlots[i].position) <= snappingDistance)
				{
                    transform.position = puzzleManager.pieceSlots[i].position;
                    transform.rotation = puzzleManager.pieceSlots[i].rotation;

                    if (i == pieceNumber) IsInCorrectSpot = true;

                    puzzleManager.CheckIfAllPicesAreInCorrectSpots();
                    break;
				}
			}

            return;
		}

        transform.position = Input.mousePosition;
    }

    // Taken from Unity forum responce by daveMennenoh and modified slightly

    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverThisUIElement()
    {
        return IsPointerOverThisUIElement(GetEventSystemRaycastResults());
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverThisUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject == gameObject)
                return true;
        }
        return false;
    }


    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
