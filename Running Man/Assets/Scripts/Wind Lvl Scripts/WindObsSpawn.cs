using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindObsSpawn : MonoBehaviour {

    public float timePassed = 0.0f, waitTime = 1.0f;

    public List<GameObject> obstacles;
    int listIndex = 0;

    void spawnObs()
    {
        obstacles.Add(GameObject.CreatePrimitive(PrimitiveType.Capsule));
        obstacles[listIndex].transform.position = gameObject.transform.position;

        //Random rotation for obstacles
        //Could use some work
        float newZRot = Random.Range(-45.0f, 45.0f);

        if (gameObject.transform.position.y >= 2.25f)
            newZRot = Random.Range(-45.0f, 30.0f);

        if (gameObject.transform.position.y <= -2.25f)
            newZRot = Random.Range(-30.0f, 45.0f);

        obstacles[listIndex].transform.rotation = new Quaternion(0, 0, newZRot, 1);
        listIndex++;

        timePassed = 0;
        waitTime = Random.Range(1, 3);

        float newYPos = Random.Range(-4.5f, 4.5f);

        gameObject.transform.position = new Vector3(transform.position.x, newYPos);
    }

    void CheckObsPos()
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            if (obstacles[i].transform.position.x <= -8.0f)
            {
                Destroy(obstacles[i]);
                obstacles.RemoveAt(i);
                listIndex--;              
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        timePassed += Time.deltaTime;

        if (timePassed >= waitTime)
            spawnObs();

        if (obstacles != null)
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                //Makes objects in "obstacles" container move in the direction they're facing
                obstacles[i].transform.position += obstacles[i].transform.right * 0.1f;
            }            
        }

        if(obstacles.Count > 0)
        CheckObsPos();
    }
}
