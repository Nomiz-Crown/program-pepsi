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

    // Method to load a scene
    public void LoadGameScene()
    {
        SceneManager.LoadScene("startcutscene");  // Replace with your target scene name
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }
}