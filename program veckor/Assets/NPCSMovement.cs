using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class NPCSMovement : MonoBehaviour
{
    Rigidbody2D rb;
    float MovementChance;
    float MovementDirection;
    float MovementAmount;







    // Start is called before the first frame update
     void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        System.Random rand = new System.Random();
            int Movement = rand.Next(1, 4);


        if (Movement == 1)
        {
            rb.velocity = new Vector2(0, 5);
        }
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

            
           
   





    }
}
