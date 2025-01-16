using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimepanelManager : MonoBehaviour
{
    public GameObject correctMessage;
    public GameObject failMessage;
    public GameObject timePanel;
    public Button correctButton;
    public Button wrongButton1;
    public Button wrongButton2;
    public HeartManager heartManager;

    void Start()
    {
        correctButton.onClick.AddListener(HandleCorrectButton);
        wrongButton1.onClick.AddListener(HandleWrongButton);
        wrongButton2.onClick.AddListener(HandleWrongButton);
    }

    void HandleCorrectButton()
    {
        correctMessage.SetActive(true);
        DisableAllButtons();
        StartCoroutine(DeactivateTimePanelAfterDelay());
    }

    void HandleWrongButton()
    {
        failMessage.SetActive(true);
        heartManager.DecreaseHeart();
        StartCoroutine(DeactivateFailMessageAfterDelay());
    }

    void DisableAllButtons()
    {
        correctButton.gameObject.SetActive(false);
        wrongButton1.gameObject.SetActive(false);
        wrongButton2.gameObject.SetActive(false);
    }

    IEnumerator DeactivateTimePanelAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        timePanel.SetActive(false);
    }

    IEnumerator DeactivateFailMessageAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        failMessage.SetActive(false);
    }
}
