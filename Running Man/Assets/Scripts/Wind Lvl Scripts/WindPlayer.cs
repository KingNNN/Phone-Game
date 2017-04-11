using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPlayer : MonoBehaviour {

    public Vector3 vel;

    public float fanSpeed, accellRate;

    public float xVel, yVel, maxVel = 0.2f;

    public float xConstraint, yConstraint;

    public bool DEBUG_fanOn = true;

    // Use this for initialization
    void Start() {

        xVel = 0;
        yVel = 0;

        fanSpeed = 0.1f;
        accellRate = 0.3f;

        vel = Vector3.zero;

    }

    void AddVelocity()
    {
        //Only runs this block of code in the editor
        //make sure to use accellormeter for final build
#if (UNITY_EDITOR)
        if (Input.GetKey("d"))
        {
            xVel += Time.deltaTime * accellRate;
        }
        
        if (Input.GetKey("w"))
        {
            yVel += Time.deltaTime * accellRate;
        }        

        
        if (Input.GetKey("s"))
        {
            yVel -= Time.deltaTime * accellRate;
        }
#endif

        if (transform.position.y > yConstraint)
        {
            transform.position = new Vector3(transform.position.x,yConstraint);
            yVel = 0;
        }

        if (transform.position.y < -yConstraint)
        {
            transform.position = new Vector3(transform.position.x, -yConstraint);
            yVel = 0;
        }

            vel = new Vector3(xVel, yVel);
            transform.position += vel;
    }

    void BalanceVelocity()
    {
        //Constant "fan" blowback
        if(DEBUG_fanOn)
        xVel -= Time.deltaTime * fanSpeed;

        //Vertical movement
        if (yVel != 0)
        {
            if (yVel > 0)
            {
                yVel -= Time.deltaTime * fanSpeed;

                if (yVel < 0.00f)
                    yVel = 0;
            }

            if (yVel < 0)
            {
                yVel += Time.deltaTime * fanSpeed;

                if (yVel > -0.00f)
                    yVel = 0;
            }

            
        }

        //Limits both velocities to a certain value (0.2f)
        if (xVel < -maxVel)
            xVel = -maxVel;
        if (xVel > maxVel)
            xVel = maxVel;

        if (yVel < -maxVel)
            yVel = -maxVel;
        if (yVel > maxVel)
            yVel = maxVel;
    }



    // Update is called once per frame
    void Update () {

        AddVelocity();
        BalanceVelocity();

        if (transform.position.x < xConstraint)
        {
            Destroy(gameObject);
        }                       

	}
}
