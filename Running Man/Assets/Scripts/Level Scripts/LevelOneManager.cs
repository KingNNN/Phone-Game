using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneManager : MonoBehaviour {

    /*
    Eventually make a variable for maxTime that gets smaller every instantiation
    that eventually gets down to something like 1.25 or something.
    Play with it a little
    */

    public Transform startPosHi, startPosLo;
    public GameObject obstacle;

    public float timePassed = 0, 
                 timeToWait = 2.0f;

    int randomNum;
	
	// Update is called once per frame
	void Update () {

        timePassed += Time.deltaTime;

        if (timePassed >= timeToWait)
        {
            randomNum = Random.Range(1,10);

            if(randomNum % 2 == 0)
                Instantiate(obstacle, startPosHi);
            else
                Instantiate(obstacle, startPosLo);

            timePassed = 0;

            if (timeToWait > 0.5f)
                timeToWait -= timeToWait * 0.1f;// Random.Range(0.5f,2.0f);
            else
                timeToWait = 0.5f;
        }

	}
}
