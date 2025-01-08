using UnityEngine;
using UnityEngine.UI;

public class AnxietyUI : MonoBehaviour
{
    public Slider anxietySlider; // Referens till slidern
    public Movement playerMovement; // Referens till spelarens Movement-script

    void Start()
    {
        // Hitta spelaren och dess Movement-script om inte redan tilldelat
        if (playerMovement == null)
        {
            playerMovement = GameObject.FindWithTag("Player").GetComponent<Movement>();
        }
    }

    void Update()
    {
        // Uppdatera slidern med spelarens nuvarande ångestnivå
        if (playerMovement != null && anxietySlider != null)
        {
            anxietySlider.value = playerMovement.anxiety;
        }
    }
}