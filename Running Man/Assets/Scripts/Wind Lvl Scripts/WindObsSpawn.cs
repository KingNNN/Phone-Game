using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindObsSpawn : MonoBehaviour {

    public float timePassed = 0.0f, waitTime = 1.0f;

    public GameObject obstacle;

    void spawnObs()
    {
        if (obstacle != null)
            Destroy(obstacle);

        obstacle = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        obstacle.transform.position = gameObject.transform.position;

        timePassed = 0;
        waitTime = Random.Range(1, 3);

        float newYPos = Random.Range(-4.5f, 4.5f);
        //Commented out random rotation code, moght put it back in later
        //float newZRot = Random.Range(-45.0f, 45.0f);

        //if (newYPos >= 2.25f)
        //    newZRot = Random.Range(-45.0f, 30.0f);

        //if (newYPos <= -2.25f)
        //    newZRot = Random.Range(-30.0f, 45.0f);

        gameObject.transform.position = new Vector3(transform.position.x, newYPos);
        //gameObject.transform.rotation = new Quaternion(0, 0, newZRot,1);
    }
	
	// Update is called once per frame
	void Update () {

        timePassed += Time.deltaTime;

        if (timePassed >= waitTime)
            spawnObs();

        if (obstacle != null)
            obstacle.transform.position += Vector3.left * 0.1f;
		
	}
}
