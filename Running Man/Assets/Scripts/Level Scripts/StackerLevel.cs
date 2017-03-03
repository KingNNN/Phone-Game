using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackerLevel : MonoBehaviour {

    public GameObject stackBlock;       //Block that is created up top
    public GameObject currentBlock;     //Block for motion check

    public Transform startBlock;        //Block at bottom (don't touch)
    public Vector3 spwnPoint,           //Place to spawn blocks
        motionCheck1,                   //Check pos 1
        motionCheck2;                   //Check pos 2

    bool startChecking = false;         //Starts motion check func
    float checkDelta = 0,               //
        checkTime = 0.75f;               //How often to check for motion

    public float dist = 7;              //Height from highest block to drop

	// Use this for initialization
	void Start () {        

        SpawnBlock();
        motionCheck1 = Vector3.zero;
        motionCheck2 = Vector3.zero;

    }

    void motionCheck()
    {
        //Here I find the dist moved by the block when it lands and after 
        //0.75 seconds. If the distance is less than the alloted amount, move on
        //to the next player. The only problem is finding the right distance
        //to allow before moving on, 0.1 isn't enough sometimes (it allows it to wobble still before
        //moving on)
        if (motionCheck1 != Vector3.zero)
        {
            checkDelta += Time.deltaTime;

            if (checkDelta >= checkTime)
            {
                motionCheck2 = currentBlock.transform.position;

                if (Vector3.Distance(motionCheck1, motionCheck2) < 0.1f)
                {
                    //SUCCESS
                    motionCheck1 = Vector3.zero;
                    motionCheck2 = Vector3.zero;
                    checkDelta = 0;
                    SpawnBlock();
                }
                else
                {
                    //FAILURE
                    motionCheck1 = motionCheck2;
                    motionCheck2 = Vector3.zero;
                    checkDelta = 0;
                }
            }
        }
    }

    void SpawnBlock()
    {
        startChecking = false;
        StackerBlock[] collection = FindObjectsOfType<StackerBlock>();

        int highest = 0;

        for (int i = 0; i < collection.Length; i++)
        {
            if (collection[i].transform.position.y > spwnPoint.y)            
                highest = i;            
        }

        if(collection.Length == 0)
            spwnPoint = new Vector3(0, startBlock.position.y + dist, 0);
        else
            spwnPoint = new Vector3(0, collection[highest].transform.position.y + dist, 0);

        Instantiate(stackBlock, spwnPoint, Quaternion.Euler(0,0,0));

        //Implement some sort of smoothness, probably from running level
        Camera.main.transform.position = new Vector3(spwnPoint.x, spwnPoint.y - 3,-10);
        startChecking = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("g"))
            SpawnBlock();

        if (startChecking)
            motionCheck();
    }
}
