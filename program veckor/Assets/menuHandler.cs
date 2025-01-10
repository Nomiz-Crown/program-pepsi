using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuHandler : MonoBehaviour
{
    public GameObject playButton;
    public GameObject settingsButton;
    public GameObject creditsButton;
    public GameObject creditsPanel;
    public GameObject settingsPanel;

    // Load the game scene
    public void LoadGameScene()
    {
        SceneManager.LoadScene("startcutscene");
    }

    // Toggle the credits panel on/off
    public void ToggleCredits()
    {
        // Switch between active and inactive states
        creditsPanel.SetActive(!creditsPanel.activeSelf);
    }

    public void ToggleSettings()
    {
        // Switch between active and inactive states
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
}