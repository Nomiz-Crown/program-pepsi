using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSMovement : MonoBehaviour
{
    Rigidbody2D rb;
<<<<<<< HEAD
    Vector2 startPosition;
    Vector2 targetPosition;
    bool isMoving = false;
    float speed = 2f; // Movement speed
    float waitTime = 5f; // Wait time between movements
    float moveDistance = 2f; // Move distance in Unity units

    void Start()
=======
    float MovementChance;
    float MovementDirection;
    float MovementAmount;







    // Start is called before the first frame update
     void Start()
>>>>>>> 132524fcbaad071f5a213db92f686b7f3bb89bfe
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
<<<<<<< HEAD
        while (true)
=======
        System.Random rand = new System.Random();
            int Movement = rand.Next(1, 4);


        if (Movement == 1)
>>>>>>> 132524fcbaad071f5a213db92f686b7f3bb89bfe
        {
            ChooseNewDirection();
            yield return new WaitUntil(() => isMoving == false);
            yield return new WaitForSeconds(waitTime);
        }
<<<<<<< HEAD
=======
        else if (Movement == 2)
        {
            rb.velocity = new Vector2(5, 0);

        }
        else if (Movement == 3)
        {
            rb.velocity = new Vector2(-5, 0);

        }
        else if (Movement == 4)
        {
            rb.velocity = new Vector2(0, -5);
        }

            
           
   





>>>>>>> 132524fcbaad071f5a213db92f686b7f3bb89bfe
    }

    void ChooseNewDirection()
    {
        int direction = Random.Range(1, 5); // 1-Up, 2-Right, 3-Left, 4-Down
        startPosition = transform.position;

        switch (direction)
        {
            case 1: // Up
                targetPosition = startPosition + Vector2.up * moveDistance;
                break;
            case 2: // Right
                targetPosition = startPosition + Vector2.right * moveDistance;
                break;
            case 3: // Left
                targetPosition = startPosition + Vector2.left * moveDistance;
                break;
            case 4: // Down
                targetPosition = startPosition + Vector2.down * moveDistance;
                break;
        }

        StartCoroutine(MoveToTarget());
    }

    IEnumerator MoveToTarget()
    {
        isMoving = true;

        while ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }
}