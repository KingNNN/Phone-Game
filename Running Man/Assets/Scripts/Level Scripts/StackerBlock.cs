﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackerBlock : MonoBehaviour {

    public float minX, maxX;

    public float speed = 5;
    float maxSpeed = 10;
    public float baseSpeed = 5;

    public bool dropFlag = false;

    public bool movingLeft = true;

    Rigidbody myRigidbody;

    bool sentData = false;

    public GameObject[] gameStack;
    public int stackIter = 0;

    int colorNumber;

    // Use this for initialization
    void Start () {

        minX = -5;
        maxX = 5;

        myRigidbody = GetComponent<Rigidbody>();

        myRigidbody.useGravity = false;

        gameStack = GameObject.Find("Level Manager").GetComponent<StackerLevel>().gameStack;
        stackIter = GameObject.Find("Level Manager").GetComponent<StackerLevel>().stackIter;
        colorNumber = GameObject.Find("Level Manager").GetComponent<StackerLevel>().blockNumber;


        gameStack[stackIter] = gameObject;

        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Specular");

        switch (colorNumber)
        {
            case 1:
                { rend.material.SetColor("_Color", Color.red); }
                break;

            case 2:
                { rend.material.SetColor("_Color", Color.blue); }
                break;

            case 3:
                { rend.material.SetColor("_Color", Color.green); }
                break;

            case 4:
                { rend.material.SetColor("_Color", Color.yellow); }
                break;

            default:
                break;
        }

        //Advances the color number
        GameObject.Find("Level Manager").GetComponent<StackerLevel>().blockNumber++;

    }

    public void addSpeed()
    { speed = (speed < maxSpeed) ? speed+=0.5f : speed+=0; }

    public void resetSpeed()
    { speed = baseSpeed; }

    void OnCollisionEnter(Collision other)
    {
        //Copies current pos when it lands into the motionCheck var in the level manager script
        //to check for wobbles and allows respawn
        if (!sentData)
        {
            GameObject.Find("Level Manager").GetComponent<StackerLevel>().motionCheck1 =
                gameObject.transform.position;

            GameObject.Find("Level Manager").GetComponent<StackerLevel>().currentBlock = gameObject;
                        
            GameObject.Find("Level Manager").GetComponent<StackerLevel>().stackIter = ++stackIter;
            sentData = true;
        }
    }

    void waitingToDrop()
    {
        if (movingLeft)
        {
            transform.Translate(Time.deltaTime * -speed, 0, 0);

            //Switches direction
            if (transform.position.x <= minX)
                movingLeft = false;

        }
        else
        {
            transform.Translate(Time.deltaTime * speed, 0, 0);

            //Switches direction
            if (transform.position.x >= maxX)
                movingLeft = true;
        }
    }
	
	// Update is called once per frame
	void Update () {

        //Moves the block side to side
        if(!dropFlag)
            waitingToDrop();

        if (Input.GetKeyDown("s"))
        {
            dropFlag = true;
            myRigidbody.useGravity = true;
        }
		
	}
}
