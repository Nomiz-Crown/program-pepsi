using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class MakeupDrag : MonoBehaviour
{
    [SerializeField] private RawImage correctMakeupImage; // The correct RawImage
    [SerializeField] private Text correctMakeupText; // The Text UI for the correct RawImage
    [SerializeField] private RawImage makeupDropTarget;
    [SerializeField] private GameObject successEffect;
    [SerializeField] private GameObject objectToHide;
    [SerializeField] private HeartManager heartManager;
    [SerializeField] private List<RawImage> incorrectMakeupImages;
    [SerializeField] private List<Text> incorrectMakeupTexts;
    [SerializeField] private List<Text> incorrectMakeupErrorTexts; // New list for wrong text UI elements

    private bool isDraggingCorrect = false;
    private List<bool> isDraggingIncorrect = new List<bool>();
    private Vector3 dragOffset;
    private Vector3 correctOriginalPos;
    private List<Vector3> incorrectOriginalPositions = new List<Vector3>();

    void Start()
    {
        AddHoverEvents(correctMakeupImage, correctMakeupText);

        for (int i = 0; i < incorrectMakeupImages.Count; i++)
        {
            incorrectMakeupTexts[i].gameObject.SetActive(false);
            incorrectMakeupErrorTexts[i].gameObject.SetActive(false); // Hide error text initially
            AddHoverEvents(incorrectMakeupImages[i], incorrectMakeupTexts[i]);
            incorrectOriginalPositions.Add(incorrectMakeupImages[i].transform.position);
            isDraggingIncorrect.Add(false);
        }

        correctMakeupText.gameObject.SetActive(false);
        correctOriginalPos = correctMakeupImage.transform.position;
    }

    void Update()
    {
        HandleDrag(correctMakeupImage, isDraggingCorrect);

        for (int i = 0; i < incorrectMakeupImages.Count; i++)
        {
            HandleDrag(incorrectMakeupImages[i], isDraggingIncorrect[i]);
        }
    }

    private void AddHoverEvents(RawImage rawImage, Text textUI)
    {
        EventTrigger trigger = rawImage.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entryEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entryEnter.callback.AddListener((eventData) => { textUI.gameObject.SetActive(true); });
        trigger.triggers.Add(entryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        entryExit.callback.AddListener((eventData) => { textUI.gameObject.SetActive(false); });
        trigger.triggers.Add(entryExit);

        EventTrigger.Entry entryDown = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        entryDown.callback.AddListener((eventData) => { StartDragging(rawImage); });
        trigger.triggers.Add(entryDown);

        EventTrigger.Entry entryUp = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        entryUp.callback.AddListener((eventData) => { StopDragging(rawImage); });
        trigger.triggers.Add(entryUp);
    }

    private void StartDragging(RawImage rawImage)
    {
        if (rawImage == correctMakeupImage)
        {
            isDraggingCorrect = true;
            dragOffset = correctMakeupImage.transform.position - Input.mousePosition;
        }
        else
        {
            for (int i = 0; i < incorrectMakeupImages.Count; i++)
            {
                if (rawImage == incorrectMakeupImages[i])
                {
                    isDraggingIncorrect[i] = true;
                    dragOffset = incorrectMakeupImages[i].transform.position - Input.mousePosition;
                }
            }
        }
    }

    private void StopDragging(RawImage rawImage)
    {
        if (rawImage == correctMakeupImage)
        {
            isDraggingCorrect = false;
            if (IsOverlapping(correctMakeupImage, makeupDropTarget))
            {
                correctMakeupImage.gameObject.SetActive(false);
                successEffect.SetActive(true);
                StartCoroutine(DeactivateAfterDelay(objectToHide, 3f));

                foreach (var wrongImage in incorrectMakeupImages)
                {
                    wrongImage.gameObject.SetActive(false);
                }
                foreach (var wrongText in incorrectMakeupTexts)
                {
                    wrongText.gameObject.SetActive(false);
                }
            }
            else
            {
                correctMakeupImage.transform.position = correctOriginalPos;
            }
        }
        else
        {
            for (int i = 0; i < incorrectMakeupImages.Count; i++)
            {
                if (rawImage == incorrectMakeupImages[i])
                {
                    isDraggingIncorrect[i] = false;
                    if (IsOverlapping(incorrectMakeupImages[i], makeupDropTarget))
                    {
                        heartManager?.DecreaseHeart();
                    }
                    incorrectMakeupImages[i].transform.position = incorrectOriginalPositions[i];

                    // Activate the error text for 2 seconds when a wrong image is dropped
                    StartCoroutine(ShowErrorText(incorrectMakeupErrorTexts[i], 5f));
                }
            }
        }
    }

    private bool IsOverlapping(RawImage dragged, RawImage target)
    {
        RectTransform draggedRect = dragged.GetComponent<RectTransform>();
        RectTransform targetRect = target.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(targetRect, draggedRect.position);
    }

    private void HandleDrag(RawImage rawImage, bool isDragging)
    {
        if (isDragging)
        {
            rawImage.transform.position = Input.mousePosition + dragOffset;
        }
    }

    private IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    private IEnumerator ShowErrorText(Text errorText, float delay)
    {
        errorText.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        errorText.gameObject.SetActive(false);
    }
}