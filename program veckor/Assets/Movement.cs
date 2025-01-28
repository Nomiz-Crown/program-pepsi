using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f; 
    private Rigidbody2D rb;

    // �ngest-relaterade variabler
    public float anxiety = 0f;
    public float maxAnxiety = 100f;
    public float anxietyIncreaseRate = 10f; // Detta anv�nds inte l�ngre
    public float anxietyDecreaseRate = 5f;  // Detta anv�nds inte l�ngre
    private bool isBreathing = false; 

    public bool isBeingChased = false; 
    public bool isHiding = false;      

    private bool hasDied = false; // Kontroll f�r att debugga kardiak arrest endast en g�ng

    private float anxietyIncreaseTimer = 0f; // Timer f�r att �ka �ngest
    private float anxietyDecreaseTimer = 0f; // Timer f�r att minska �ngest
    Animator animator;
    int idledirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isBeingChased = false;
        isHiding = false;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            animator.Play("backshots");
            idledirection = 1;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            animator.Play("sideprofileplayer1");
            idledirection = 2;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            animator.Play("walking forward player");
            idledirection = 3;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            animator.Play("playersideprofle walking");
            idledirection = 4;
        }
        else
        {
            if (idledirection == 3)
            {
                animator.Play("idle animation");
            }
            if (idledirection == 4)
            {
                animator.Play("rightidle");
            }
            if (idledirection == 1)
            {
                animator.Play("backidle");
            }
            if (idledirection == 2)
            {
                animator.Play("idleleft");
            }
        }

        // R�relse
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical);
        rb.velocity = movement * speed;

        // Hantera �ngest�kning och andning
        if (!isBreathing && isBeingChased || isHiding)
        {
            // �ka �ngest varje 0.1 sekund n�r spelaren inte andas
            anxietyIncreaseTimer += Time.deltaTime;
            if (anxietyIncreaseTimer >= 0.1f)
            {
                anxiety += 1;
                anxietyIncreaseTimer = 0f; // Nollst�ll timern
            }
            if (anxiety < 70)
            {
                rb.velocity = movement * speed * 1.5f;
            }
            else
            {
                rb.velocity = movement * speed * (1.5f - anxiety / 200);
            }
        }
        else
        {
            // Minska �ngest varje 0.05 sekund n�r spelaren andas
            anxietyDecreaseTimer += Time.deltaTime;
            if (anxietyDecreaseTimer >= 0.05f)
            {
                anxiety -= 1;
                anxietyDecreaseTimer = 0f; // Nollst�ll timern
            }
        }

        // Begr�nsa �ngestniv�n
        anxiety = Mathf.Clamp(anxiety, 0, maxAnxiety);

        // Kontrollera kardiak arrest
        if (anxiety >= maxAnxiety && !hasDied)
        {
            hasDied = true; // Markera att spelaren har d�tt
            Debug.Log("Karakt�ren dog av kardiak arrest!");
            // L�gg till d�dslogik h�r
        }

        // Hantera andning
        if (Input.GetKey(KeyCode.Space))
        {
            isBreathing = true;
            speed = 3f;
        }
        else
        {
            speed = 5f;
            isBreathing = false;
        }
    }
}