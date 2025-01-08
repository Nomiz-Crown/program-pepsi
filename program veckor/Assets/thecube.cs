using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thecube : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(10f, 20f, 30f); 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
