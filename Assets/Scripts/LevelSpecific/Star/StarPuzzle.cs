using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class StarPuzzle : MonoBehaviour
{
    public UnityEvent OnWindowClosed;
    public UnityEvent OnPuzzleSolved;
    public PuzzleSlider[] sliders;

    // Start is called before the first frame update
    void Start()
    {
        if(sliders.Length != 5)
        {
            Debug.LogError("Incorrect slider amount");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnWindowClosed?.Invoke();
        }

        bool wasPuzzleSolved = true;

        for(int i = 0; i < sliders.Length; i++)
        {
            if (sliders[i].slider.value != sliders[i].targetValue)
            {
                wasPuzzleSolved = false;
                break;
            }
        }

        if(wasPuzzleSolved)
        {
            OnWindowClosed?.Invoke();
            OnPuzzleSolved?.Invoke();
        }
    }

    [System.Serializable]
    public struct PuzzleSlider
    {
        public float targetValue;
        public Slider slider;
    }
}
