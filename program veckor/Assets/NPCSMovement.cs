using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD

=======
>>>>>>> 3b43d4a28f3bff5808cad040a1c5416b0fa1f01f

public class NPCSMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 startPosition;
    Vector2 targetPosition;
    bool isMoving = false;
    float speed = 2f; // Movement speed
    float waitTime = 5f; // Wait time between movements
    float moveDistance = 2f; // Move distance in Unity units

    void Start()
<<<<<<< HEAD
=======
=======
    float MovementChance;
    float MovementDirection;
    float MovementAmount;
    // Start is called before the first frame update
<<<<<<< HEAD
    void Start()
=======
     void Start()
>>>>>>> 132524fcbaad071f5a213db92f686b7f3bb89bfe
>>>>>>> 3b43d4a28f3bff5808cad040a1c5416b0fa1f01f
>>>>>>> e3c32d69697957e8f07862d32c29b46bd52ee025
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
<<<<<<< HEAD
        while (true)
=======
<<<<<<< HEAD


        // Denna kod bestämmer OM NPCn ska röra på sig
        Unity.Mathematics.Random rand = new Unity.Mathematics.Random();
        float MovementChance = rand.NextInt(1, 2);

        // Denna Kod bestämmer vilket håll NPCn ska röra på sig.
        Unity.Mathematics.Random ran = new Unity.Mathematics.Random();
        float MovementDirection = ran.NextInt(1, 4);
        // Denna kod bstämmer hur länge den kommer  röra på sig
        Unity.Mathematics.Random ra = new Unity.Mathematics.Random();
        float MovementAmount = ra.NextInt(1, 3);
        if (MovementDirection == 1)
=======
<<<<<<< HEAD
        while (true)
=======
        System.Random rand = new System.Random();
            int Movement = rand.Next(1, 4);


        if (Movement == 1)
>>>>>>> 132524fcbaad071f5a213db92f686b7f3bb89bfe
>>>>>>> 3b43d4a28f3bff5808cad040a1c5416b0fa1f01f
>>>>>>> e3c32d69697957e8f07862d32c29b46bd52ee025
        {
            ChooseNewDirection();
            yield return new WaitUntil(() => isMoving == false);
            yield return new WaitForSeconds(waitTime);
        }
<<<<<<< HEAD
=======
<<<<<<< HEAD
        else if (MovementDirection == 2)
=======
<<<<<<< HEAD
=======
        else if (Movement == 2)
>>>>>>> 3b43d4a28f3bff5808cad040a1c5416b0fa1f01f
        {
            rb.velocity = new Vector2(5, 0);

        }
        else if (MovementDirection == 3)
        {
            rb.velocity= new Vector2(-5, 0);

        }
        else if (MovementDirection == 4)
        {
            rb.velocity= new Vector2(0, -5);
        }

            
            
   





>>>>>>> 132524fcbaad071f5a213db92f686b7f3bb89bfe
>>>>>>> e3c32d69697957e8f07862d32c29b46bd52ee025
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