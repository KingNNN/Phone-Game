using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackerLevel : MonoBehaviour {

    public GameObject stackBlock;       //Block that is created up top
    public GameObject currentBlock;     //Block for motion check

    public GameObject[] gameStack; 
    public int stackIter = 0;

    public Transform startBlock;        //Block at bottom (don't touch)
    public Vector3 spwnPoint,           //Place to spawn blocks
        motionCheck1,                   //Check pos 1
        motionCheck2;                   //Check pos 2

    bool startChecking = false;         //Starts motion check func
    float checkDelta = 0,               //
        checkTime = 0.75f;               //How often to check for motion

    public float dist = 7,              //Height from highest block to drop
                 camZPos;

    public int blockNumber = 1;

	// Use this for initialization
	void Start () {
        
        gameStack = new GameObject[1];
        stackBlock.GetComponent<StackerBlock>().resetSpeed();

        camZPos = -10;
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

                    //if (stackIter > 0)
                    //{
                    //    for (int i = 0; i < stackIter; i++)
                    //    {
                    //        //Not the correct way to stabalize, but its all I have for now,
                    //        //after a while, blocks start floating...
                    //        //fix later
                    //        gameStack[i].GetComponent<Rigidbody>().useGravity = false;
                    //    }
                    //}

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
        Debug.Log("Current Player: " + blockNumber);

        startChecking = false;
        StackerBlock[] collection = FindObjectsOfType<StackerBlock>();        

        int highest = 0;

        for (int i = 0; i < collection.Length; i++)
        {
            if (collection[i].transform.position.y > spwnPoint.y)            
                highest = i;            
        }

        if(collection.Length == 0)
            spwnPoint = new Vector3(0, startBlock.position.y + (dist + 2), 0);
        else
            spwnPoint = new Vector3(0, collection[highest].transform.position.y + dist, 0);
        
        /*******************************************************/
        //Change color of each block in the StackerBlock script//        
        /*******************************************************/

        Instantiate(stackBlock, spwnPoint, Quaternion.Euler(0,0,0));
        System.Array.Resize(ref gameStack, (stackIter +1));
        stackBlock.GetComponent<StackerBlock>().addSpeed();  

        //Implement some sort of smoothness, probably from running level        
        Camera.main.transform.position = new Vector3(spwnPoint.x, spwnPoint.y - 3,camZPos);
        //camZPos -= 0.5f;
        startChecking = true;

        
    }
	
	// Update is called once per frame
	void Update () {

        //Changes color of blocks
        if (blockNumber >= 5)
            blockNumber = 1;

        //Debug spawner
        if (Input.GetKeyDown("g"))
            SpawnBlock();

        /*******************************************************/
        /*NEED TO FIGURE OUT A WAY TO RESTRICT THE SWAY MOTIONS*/
        /*******************************************************/

        /*                           !!!                       */
        /*MIGHT IMPLEMENT A COUNTDOWN UNTIL NEXT PLAYER TO HELP*/
        /*******************************************************/
        if (startChecking)
        {
            motionCheck();

            if (stackIter > 0)
            {
                for (int i = 0; i < stackIter; i++)
                {
                    //Not the correct way to stabalize, but its all I have for now,
                    //after a while, blocks start floating...
                    //fix later
                    gameStack[i].GetComponent<Rigidbody>().useGravity = true;
                    gameStack[i].GetComponent<Rigidbody>().isKinematic = false;
                }
            }

        }
        else
        {
            if (stackIter > 0)
            {
                for (int i = 0; i < stackIter; i++)
                {
                    //Not the correct way to stabalize, but its all I have for now,
                    //after a while, blocks start floating...
                    //fix later
                    gameStack[i].GetComponent<Rigidbody>().useGravity = false;
                    gameStack[i].GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }
}
