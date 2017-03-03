using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneManager : MonoBehaviour {

    /*
    Eventually make a variable for maxTime that gets smaller every instantiation
    that eventually gets down to something like 1.25 or something.
    Play with it a little
    */

    public Transform startPos;
    public GameObject obstacle;

    public float timePassed = 0, 
                 timeToWait = 2.0f;
	
	// Update is called once per frame
	void Update () {

        timePassed += Time.deltaTime;

        if (timePassed >= timeToWait)
        {
            Instantiate(obstacle, startPos);
            timePassed = 0;

            timeToWait = Random.Range(1.0f,3.0f);
        }

	}
}
