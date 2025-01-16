using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class casefileDrag : MonoBehaviour
{
    [SerializeField] private RawImage rawImage1; // The correct RawImage
    [SerializeField] private Text textUI1; // The Text UI for the correct RawImage
    [SerializeField] private RawImage dropTargetImage;
    [SerializeField] private GameObject newGameObject;
    [SerializeField] private GameObject toDeactivate;
    [SerializeField] private HeartManager heartManager; // Reference to HeartManager
    [SerializeField] private List<RawImage> wrongRawImages; // List of wrong RawImages
    [SerializeField] private List<Text> wrongTextUIs; // List of Text UIs corresponding to wrong RawImages

    private bool isDragging1 = false;
    private List<bool> isDraggingWrong = new List<bool>(); // List to track dragging state for wrong options
    private Vector3 offset;
    private Vector3 originalPos1;
    private List<Vector3> originalWrongPositions = new List<Vector3>(); // List to store original positions of wrong RawImages

    void Start()
    {
        // Setup correct RawImage (rawImage1) hover and drag events
        AddHoverEvents(rawImage1, textUI1);

        // Setup each wrong RawImage and its corresponding Text UI
        for (int i = 0; i < wrongRawImages.Count; i++)
        {
            // Attach text UI to the same RawImage in the same list
            wrongTextUIs[i].gameObject.SetActive(false); // Hide the text initially
            AddHoverEvents(wrongRawImages[i], wrongTextUIs[i]);
            originalWrongPositions.Add(wrongRawImages[i].transform.position);
            isDraggingWrong.Add(false);
        }

        textUI1.gameObject.SetActive(false);
        originalPos1 = rawImage1.transform.position;
    }

    void Update()
    {
        // Handle dragging logic for the correct RawImage
        HandleDrag(rawImage1, isDragging1);

        // Handle dragging logic for the wrong RawImages
        for (int i = 0; i < wrongRawImages.Count; i++)
        {
            HandleDrag(wrongRawImages[i], isDraggingWrong[i]);
        }
    }

    private void AddHoverEvents(RawImage rawImage, Text textUI)
    {
        EventTrigger trigger = rawImage.gameObject.AddComponent<EventTrigger>();

        // Show Text UI when hovering over RawImage
        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((eventData) => { textUI.gameObject.SetActive(true); });
        trigger.triggers.Add(entryEnter);

        // Hide Text UI when pointer exits RawImage
        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((eventData) => { textUI.gameObject.SetActive(false); });
        trigger.triggers.Add(entryExit);

        // Start dragging on pointer down
        EventTrigger.Entry entryDown = new EventTrigger.Entry();
        entryDown.eventID = EventTriggerType.PointerDown;
        entryDown.callback.AddListener((eventData) => { StartDragging(rawImage); });
        trigger.triggers.Add(entryDown);

        // Stop dragging on pointer up
        EventTrigger.Entry entryUp = new EventTrigger.Entry();
        entryUp.eventID = EventTriggerType.PointerUp;
        entryUp.callback.AddListener((eventData) => { StopDragging(rawImage); });
        trigger.triggers.Add(entryUp);
    }

    private void StartDragging(RawImage rawImage)
    {
        if (rawImage == rawImage1)
        {
            isDragging1 = true;
            offset = rawImage1.transform.position - Input.mousePosition;
        }
        else
        {
            for (int i = 0; i < wrongRawImages.Count; i++)
            {
                if (rawImage == wrongRawImages[i])
                {
                    isDraggingWrong[i] = true; // Update dragging state for wrong options
                    offset = wrongRawImages[i].transform.position - Input.mousePosition;
                }
            }
        }
    }

    private void StopDragging(RawImage rawImage)
    {
        if (rawImage == rawImage1)
        {
            isDragging1 = false;
            if (IsOverlapping(rawImage1, dropTargetImage))
            {
                rawImage1.gameObject.SetActive(false);
                newGameObject.SetActive(true);
                StartCoroutine(DeactivateAfterDelay(toDeactivate, 3f));

                // Deactivate all wrong RawImages when the correct one is dropped
                foreach (var wrongRawImage in wrongRawImages)
                {
                    wrongRawImage.gameObject.SetActive(false);
                }

                // Optionally, deactivate all wrong Text UI as well if needed
                foreach (var wrongText in wrongTextUIs)
                {
                    wrongText.gameObject.SetActive(false);
                }
            }
            else
            {
                rawImage1.transform.position = originalPos1;
            }
        }
        else
        {
            for (int i = 0; i < wrongRawImages.Count; i++)
            {
                if (rawImage == wrongRawImages[i])
                {
                    isDraggingWrong[i] = false; // Stop dragging state for wrong options
                    if (IsOverlapping(wrongRawImages[i], dropTargetImage))
                    {
                        if (heartManager != null)
                        {
                            heartManager.DecreaseHeart();
                        }
                    }
                    wrongRawImages[i].transform.position = originalWrongPositions[i];
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
            rawImage.transform.position = Input.mousePosition + offset;
        }
    }

    private IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}