using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]  // This makes the class editable in the Inspector
class DialogueLine
{
    [TextArea(3, 10)]
    public string lineText;          // The dialogue text
    public bool changeImage;         // Should the image change?
    public RawImage imageToDisable;  // Image to hide
    public RawImage imageToEnable;   // Image to show
}

public class cutscenescript : MonoBehaviour
{
    public string scene;

    [SerializeField] private Text dialogueText;      // Legacy Text UI
    [SerializeField] private Button continueButton;  // Continue Button
    [SerializeField] private float typingSpeed = 0.05f;

    [SerializeField] private DialogueLine[] dialogueLines; // Dialogue array with image options

    private int currentLineIndex = 0;
    private bool isTyping = false;

    void Start()
    {
        continueButton.onClick.AddListener(OnContinueClicked);
        continueButton.gameObject.SetActive(false); // Hide button initially
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
        continueButton.gameObject.SetActive(true); // Show button after typing
    }

    private void OnContinueClicked()
    {
        if (isTyping) return;

        continueButton.gameObject.SetActive(false);

        DialogueLine currentLine = dialogueLines[currentLineIndex];

        // Handle image change if enabled
        if (currentLine.changeImage)
        {
            if (currentLine.imageToDisable != null)
                currentLine.imageToDisable.gameObject.SetActive(false);

            if (currentLine.imageToEnable != null)
                currentLine.imageToEnable.gameObject.SetActive(true);
        }

        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueText.text = "";  // Optional: Clear text after finishing
            continueButton.gameObject.SetActive(false);  // Hide button

            // Load the new scene after dialogue ends
            SceneManager.LoadScene(scene);  // Replace with your scene name
        }
    }
}