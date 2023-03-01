using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.PlayerSettings;

public class boidManager : MonoBehaviour
{
    [SerializeField] float yMax = 30f;

    [SerializeField] float yMin = 0f;

    [SerializeField] float xzMax = 30f;

    [SerializeField] float xzMin = -30f;
    
    [SerializeField] float visualRange = 30f;

    [SerializeField] float coherence = .5f;

    [SerializeField] float separation = .01f;

    [SerializeField] float alignment = .1f;

    [SerializeField] float maxAccel = .001f;

    [SerializeField] float minDistance = 10f;

    [SerializeField] float boundsWeight = 1f;

    [SerializeField] GameObject boid;
    
    [SerializeField] int boidNum = 15;
    
    boid[] boids;
    [SerializeField] float maxVel = 20f;

    // Start is called before the first frame update
    void Start()
    {
        boids = new boid[boidNum];
        for (int i = 0; i < boidNum; i++)
        {
            boids[i] = Instantiate(boid, new Vector3((int)UnityEngine.Random.Range(xzMin, xzMax), (int)UnityEngine.Random.Range(yMin, yMax), (int)UnityEngine.Random.Range(xzMin, xzMax)), Quaternion.identity).GetComponent<boid>();
            boids[i].rb.velocity = new Vector3((int)UnityEngine.Random.Range(xzMin, xzMax), (int)UnityEngine.Random.Range(yMin, yMax));
            boids[i].rb.velocity = boids[i].rb.velocity.normalized * maxVel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Console.WriteLine(boids);
        for (int i = 0; i < boidNum; i++)
        {
            boids[i].rb.velocity += CalculateAcceleration(boids[i]) * Time.deltaTime;
            if (boids[i].rb.velocity.magnitude > maxVel)
            {
                boids[i].rb.velocity = boids[i].rb.velocity.normalized * maxVel;
            }
        }
    }

    Vector3 CalculateAcceleration(boid character)
    {
        Vector3 result = Vector3.zero;

        Vector3 cohere = Vector3.zero;

        Vector3 separate = Vector3.zero;

        Vector3 match = Vector3.zero;

        int neighbors = 0;

        for (int i = 0; i < boidNum; i++)
        {
            if (boids[i] == character)
            {
                continue;
            }
            if ((boids[i].t.position - character.t.position).magnitude < visualRange)
            {
                cohere += boids[i].t.position;
                match += boids[i].rb.velocity;
                neighbors++;
            }
            if ((boids[i].t.position - character.t.position).magnitude < minDistance)
            {
                separate += character.t.position - boids[i].t.position;
            }


        }

        if (neighbors > 0)
        {
            cohere /= neighbors;
            match /= neighbors;
        }
        Vector3 res = Vector3.zero;
        if (character.transform.position.y > yMax)
        {
            res += Vector3.down * Mathf.Abs(character.t.position.y - yMax);
        }
        else if (character.transform.position.y < yMin)
        {
            res += Vector3.up * Mathf.Abs(character.t.position.y - yMin);
        }
        if (character.transform.position.x > xzMax)
        {
            res += Vector3.left * Mathf.Abs(character.t.position.x - xzMax);
        }
        else if (character.transform.position.x < xzMin)
        {
            res += Vector3.right * Mathf.Abs(character.t.position.x - xzMin);
        }
        if (character.transform.position.z > xzMax)
        {
            res += Vector3.back * Mathf.Abs(character.t.position.z - xzMax);
        }
        else if (character.transform.position.z < xzMin)
        {
            res += Vector3.forward * Mathf.Abs(character.t.position.z - xzMin);
        }
        

        result = cohere * coherence + match * alignment + separate * separation + res * boundsWeight;
        return result;
    }
}
