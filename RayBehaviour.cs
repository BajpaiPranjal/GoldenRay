using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RayBehaviour : MonoBehaviour
{
    //Script refrences
    

    //Components
    LineRenderer rend;
    EdgeCollider2D edgeCollider;
    AudioSource audioSource;

    //Variables
    public List<Vector2> linePoints = new List<Vector2>();

    public Material[] materials = new Material[4];
    public Material defaultRayMaterial;
    
    int count = 0;
    public float spriteTimeCounter, timeBtwSprite;
    private bool hasPickUp;
    Color sc;

    public event Action IncChainLength;
    void Start()
    {
        rend = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        sc = rend.startColor;


        // scoreManager = FindObjectOfType<ScoreManager>();
        audioSource = GetComponent<AudioSource>();

        
    }

    // Update is called once per frame
    void Update()
    {
        linePoints[0] = rend.GetPosition(0); 
        linePoints[1] = rend.GetPosition(1);

        edgeCollider.SetPoints(linePoints);


        if (0 > spriteTimeCounter && hasPickUp)
        {
            // cycling through material
            spriteTimeCounter = timeBtwSprite;
            rend.material = materials[count];  //Debug.Log("Name: " + rend.material.name);
            count++;
            if (count > 3) count = 0;
        }

        spriteTimeCounter -= Time.deltaTime;
    }

    public void MakeRayElectric()
    {
        StartCoroutine(ElectrifyRay());
    }


    IEnumerator ElectrifyRay()
    {
        
        // make ray Electric
        //changing color and width
          // saving initial color...
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        players[0].speed = players[0].dashSpeed;
        players[1].speed = players[1].dashSpeed;

        rend.startColor = Color.white; rend.endColor = Color.white;
        rend.startWidth = 1; rend.endWidth = 1;

        hasPickUp = true;

        yield return new WaitForSeconds(4);  // run Electric ray animation 

        // make ray Solid again
        // reverting color and width 
        rend.startColor = sc; rend.endColor = sc; // returning to initail colors...
        rend.startWidth = 0.3f; rend.endWidth = 0.3f;

        rend.material = defaultRayMaterial;
         
        hasPickUp = false;
        players[0].speed = players[0].resetSpeed;
        players[1].speed = players[1].resetSpeed;
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            audioSource.Play();
            IncChainLength();
            //Debug.Log("Action called!");
        }
    }
}
