using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hugoplayermovement : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -9.2f)
        {
            rb.velocity = new Vector2(0,-5);
        }
        if (transform.position.y < -11)
        {
            rb.velocity = new Vector2(-5, -2);
        }
        if (transform.position.y < -11 && transform.position.x > -2.5f)
        {
            rb.velocity = new Vector2(0, -5);
        }
        if (transform.position.y < -17)
        {
            rb.velocity = new Vector2(-5, 0);
        }
        if (transform.position.x < -16)
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
    public void walktoroom()
    {

        rb.velocity = new Vector2(-5,0);
    }
}
