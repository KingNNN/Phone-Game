using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPlayer : MonoBehaviour {

    public Vector3 vel;

    public float fanSpeed, accellRate;

    public float xVel, yVel;

    public float xConstraint, yConstraint;

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
        if (Input.GetKey("space"))
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
        

        if (transform.position.y > yConstraint)
        {
            transform.position = new Vector3(transform.position.x,yConstraint);
        }

        if (transform.position.y < -yConstraint)
        {
            transform.position = new Vector3(transform.position.x, -yConstraint);
        }

            vel = new Vector3(xVel, yVel);
        transform.position += vel;
    }

    void BalanceVelocity()
    {
        xVel -= Time.deltaTime * fanSpeed;

        if (yVel != 0)
        {
            if (yVel > 0)
            {
                yVel -= Time.deltaTime * fanSpeed;
            }

            if (yVel < 0)
            {
                yVel += Time.deltaTime * fanSpeed;
            }
        }
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
