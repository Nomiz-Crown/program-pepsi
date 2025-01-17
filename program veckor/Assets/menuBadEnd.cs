using UnityEngine;
using UnityEngine.SceneManagement;

public class menuBadEnd : MonoBehaviour
{
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("menu");
    }

    public void LoadSumSumScene()
    {
        SceneManager.LoadScene("sumsum");
    }
}