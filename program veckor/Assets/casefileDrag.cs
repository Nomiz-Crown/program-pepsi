using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections; // For coroutines

public class casefileDrag : MonoBehaviour
{
    [SerializeField] private RawImage rawImage1;
    [SerializeField] private Text textUI1;
    [SerializeField] private RawImage rawImage2;
    [SerializeField] private Text textUI2;
    [SerializeField] private RawImage dropTargetImage; // Non-draggable drop target
    [SerializeField] private GameObject newGameObject; // GameObject to activate after image 1 is dropped
    [SerializeField] private GameObject toDeactivate; // GameObject to deactivate after 3 seconds

    private bool isDragging1 = false;
    private bool isDragging2 = false;
    private Vector3 offset;
    private Vector3 originalPos1;
    private Vector3 originalPos2;

    void Start()
    {
        AddHoverEvents(rawImage1, textUI1);
        AddHoverEvents(rawImage2, textUI2);

        textUI1.gameObject.SetActive(false);
        textUI2.gameObject.SetActive(false);

        originalPos1 = rawImage1.transform.position;
        originalPos2 = rawImage2.transform.position;
    }

    void Update()
    {
        HandleDrag(rawImage1, ref isDragging1);
        HandleDrag(rawImage2, ref isDragging2);
    }

    private void AddHoverEvents(RawImage rawImage, Text textUI)
    {
        EventTrigger trigger = rawImage.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((eventData) => { textUI.gameObject.SetActive(true); });
        trigger.triggers.Add(entryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((eventData) => { textUI.gameObject.SetActive(false); });
        trigger.triggers.Add(entryExit);

        EventTrigger.Entry entryDown = new EventTrigger.Entry();
        entryDown.eventID = EventTriggerType.PointerDown;
        entryDown.callback.AddListener((eventData) => { StartDragging(rawImage); });
        trigger.triggers.Add(entryDown);

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
        else if (rawImage == rawImage2)
        {
            isDragging2 = true;
            offset = rawImage2.transform.position - Input.mousePosition;
        }
    }

    private void StopDragging(RawImage rawImage)
    {
        if (rawImage == rawImage1)
        {
            isDragging1 = false;
            if (IsOverlapping(rawImage1, dropTargetImage))
            {
                rawImage1.gameObject.SetActive(false); // Deactivate rawImage1
                rawImage2.gameObject.SetActive(false); // Deactivate rawImage2
                newGameObject.SetActive(true); // Activate the new GameObject

                StartCoroutine(DeactivateAfterDelay(toDeactivate, 3f)); // Deactivate another GameObject after 3 seconds
            }
            else
            {
                rawImage1.transform.position = originalPos1;
            }
        }
        else if (rawImage == rawImage2)
        {
            isDragging2 = false;
            if (!IsOverlapping(rawImage2, dropTargetImage))
                rawImage2.transform.position = originalPos2;
        }
    }

    private bool IsOverlapping(RawImage dragged, RawImage target)
    {
        RectTransform draggedRect = dragged.GetComponent<RectTransform>();
        RectTransform targetRect = target.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(targetRect, draggedRect.position);
    }

    private void HandleDrag(RawImage rawImage, ref bool isDragging)
    {
        if (isDragging)
        {
            rawImage.transform.position = Input.mousePosition + offset;
        }
    }

    // Coroutine to deactivate the game object after a delay
    private IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}