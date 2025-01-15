using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
class DialogueLine
{
    [TextArea(3, 10)]
    public string lineText;
    public bool changeImage;
    public RawImage imageToDisable;
    public RawImage imageToEnable;
    public bool triggerEvent;
    public GameObject eventObjectToActivate; // New field for event GameObject
}

public class cutscenescript : MonoBehaviour
{
    public string scene;

    [SerializeField] private Text dialogueText;
    [SerializeField] private Button continueButton;
    [SerializeField] private float typingSpeed = 0.05f;

    [SerializeField] private DialogueLine[] dialogueLines;

    private int currentLineIndex = 0;
    private bool isTyping = false;

    void Start()
    {
        continueButton.onClick.AddListener(OnContinueClicked);
        continueButton.gameObject.SetActive(false);
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";

        DialogueLine currentLine = dialogueLines[currentLineIndex];

        foreach (char letter in currentLine.lineText.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        if (currentLine.triggerEvent && currentLine.eventObjectToActivate != null)
        {
            currentLine.eventObjectToActivate.SetActive(true);
        }

        continueButton.gameObject.SetActive(true);
    }

    private void OnContinueClicked()
    {
        if (isTyping) return;

        continueButton.gameObject.SetActive(false);

        DialogueLine currentLine = dialogueLines[currentLineIndex];

        if (currentLine.changeImage)
        {
            if (currentLine.imageToDisable != null)
                currentLine.imageToDisable.gameObject.SetActive(false);

            if (currentLine.imageToEnable != null)
                currentLine.imageToEnable.gameObject.SetActive(true);
        }

        if (currentLine.triggerEvent)
        {
            TriggerEvent();
        }

        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueText.text = "";
            continueButton.gameObject.SetActive(false);
            SceneManager.LoadScene(scene);
        }
    }

    private void TriggerEvent()
    {
        Debug.Log("Event Triggered!");
    }
}
