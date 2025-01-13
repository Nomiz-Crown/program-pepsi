using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


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
        {
            rb.velocity = new Vector2(0, 5);
        }
        else if (MovementDirection == 2)
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

            
            
   





    }
}
