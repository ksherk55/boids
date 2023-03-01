using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boid : MonoBehaviour
{
    public Transform t;
    public Rigidbody rb;

    
    
    // Start is called before the first frame update
    void Awake()
    {
        t = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
      

    }

    
    
   
}
