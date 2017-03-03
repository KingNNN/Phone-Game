using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackerBlock : MonoBehaviour {

    public float minX, maxX;

    public float speed = 5;

    public bool dropFlag = false;

    public bool movingLeft = true;

    Rigidbody myRigidbody;

    bool sentData = false;

	// Use this for initialization
	void Start () {

        minX = -5;
        maxX = 5;

        myRigidbody = GetComponent<Rigidbody>();

        myRigidbody.useGravity = false;
		
	}

    void OnCollisionEnter(Collision other)
    {
        //Copies current pos when it lands into the motionCheck var in the level manager script
        //to check for wobbles and allows respawn
        if (!sentData)
        {
            GameObject.Find("Level Manager").GetComponent<StackerLevel>().motionCheck1 =
                gameObject.transform.position;

            GameObject.Find("Level Manager").GetComponent<StackerLevel>().currentBlock = gameObject;
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
