using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {

    public Vector3 velVec = Vector3.zero;

    public ParticleSystem flameThrower;

    public float speed = 0,
        maxSpeed = 0.5f;

	// Use this for initialization
	void Start () {
		
	}

    void MoveTank()
    {
        //Speed Control
        if (Input.GetKey("w") && speed <= maxSpeed)
            speed += Time.deltaTime;
        else
            speed -= Time.deltaTime;

        if (speed < 0)
            speed = 0;

        //Rotation Control
        if (Input.GetKey("a"))
            transform.Rotate(0, -1, 0);

        if (Input.GetKey("d"))
            transform.Rotate(0, 1, 0);

        if (Input.GetKey("space"))
            flameThrower.Play();
        else
        {
            flameThrower.Pause();
            flameThrower.Clear();
        }

        //So much easier in Unity
        //Less math to move in a straight line
        transform.Translate(Vector3.forward * speed);
    }
	
	// Update is called once per frame
	void Update () {

        MoveTank();        

    }
}
