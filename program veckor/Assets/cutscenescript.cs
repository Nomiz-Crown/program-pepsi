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
    public GameObject eventObjectToActivate;
}

public class cutscenescript : MonoBehaviour
{
    [Tooltip("Name of the scene to load after the dialogue ends")]
    public string sceneToLoad;  // Scene name to load

    [Tooltip("Enable this to load the scene after the dialogue ends")]
    public bool loadSceneAfterDialogue = true;  // Control whether the scene loads

    [Tooltip("The panel that contains the dialogue UI")]
    public GameObject dialoguePanel;  // Reference to the dialogue panel

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

            // Deactivate the dialogue panel when dialogue ends
            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(false);
            }

            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        if (loadSceneAfterDialogue)  // Check if scene loading is enabled
        {
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogWarning("Scene name is not set in the Inspector!");
            }
        }
        else
        {
            Debug.Log("Scene loading is disabled.");
        }
    }

    private void TriggerEvent()
    {
        Debug.Log("Event Triggered!");
    }
}
