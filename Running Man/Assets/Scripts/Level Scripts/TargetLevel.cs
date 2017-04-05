using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TargetLevel : MonoBehaviour {

    /*
    This is for the level that lets the player tap the screen frantically to see
    who can destroy the most targets in the alloted time. Remember to refer back to the test scene
    and the online tut. to see how to make all of the network stuff sync up across all players
    */

    public GameObject normalTarget;
    float xConstraint = 6.75f, yConstraint = 6;
    float timePassed, timeToWait;

    public Text myScoreText;

    public int localPlayerScore = 0;
    

    void Start()
    {
        timePassed = 0;
        timeToWait = 0.5f;

        for (int i = 0; i < 10; i++)
        {
            newTarget();
        }
    }

    public void addToScore(int amt)
    {
        localPlayerScore += amt;
        myScoreText.color = Color.blue;
        myScoreText.text = "Score: " + localPlayerScore;
    }

    void newTarget()
    {
        GameObject tempObj = GameObject.CreatePrimitive(PrimitiveType.Cube);

        tempObj.transform.position = new Vector3(Random.Range(-xConstraint, xConstraint),
                                          Random.Range(-4, yConstraint),0);

        tempObj.transform.LookAt(Camera.main.transform);

        //Instantiate(tempObj, tempVec, Quaternion.Euler(Vector3.zero));
    }

    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0))
        {
            if (hit.transform.name == "Cube" || hit.transform.name == "Cube(Clone)")
            {
                Destroy(hit.transform.gameObject);
                addToScore(5);
            }
        }
    }

    void Update()
    {
        timePassed += Time.deltaTime;

        if (timePassed >= timeToWait)
        {
            newTarget();
            timePassed = 0;
        }
    }
}
