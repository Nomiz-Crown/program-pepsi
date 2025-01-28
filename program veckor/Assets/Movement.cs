using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 5f; 
    private Rigidbody2D rb;

    // Ångest-relaterade variabler
    public float anxiety = 0f;
    public float maxAnxiety = 100f;
    public float anxietyIncreaseRate = 10f; // Detta används inte längre
    public float anxietyDecreaseRate = 5f;  // Detta används inte längre
    private bool isBreathing = false; 

    public bool isBeingChased = false; 
    public bool isHiding = false;      

    private bool hasDied = false; // Kontroll för att debugga kardiak arrest endast en gång

    private float anxietyIncreaseTimer = 0f; // Timer för att öka ångest
    private float anxietyDecreaseTimer = 0f; // Timer för att minska ångest
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

        // Rörelse
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical);
        rb.velocity = movement * speed;

        // Hantera ångestökning och andning
        if (!isBreathing && isBeingChased || isHiding)
        {
            // Öka ångest varje 0.1 sekund när spelaren inte andas
            anxietyIncreaseTimer += Time.deltaTime;
            if (anxietyIncreaseTimer >= 0.1f)
            {
                anxiety += 1;
                anxietyIncreaseTimer = 0f; // Nollställ timern
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
            // Minska ångest varje 0.05 sekund när spelaren andas
            anxietyDecreaseTimer += Time.deltaTime;
            if (anxietyDecreaseTimer >= 0.05f)
            {
                anxiety -= 1;
                anxietyDecreaseTimer = 0f; // Nollställ timern
            }
        }

        // Begränsa ångestnivån
        anxiety = Mathf.Clamp(anxiety, 0, maxAnxiety);

        // Kontrollera kardiak arrest
        if (anxiety >= maxAnxiety && !hasDied)
        {
            hasDied = true; // Markera att spelaren har dött
            Debug.Log("Karaktären dog av kardiak arrest!");
            // Lägg till dödslogik här
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