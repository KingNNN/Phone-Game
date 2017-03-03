using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour {

    private GameObject player;
    public bool alreadyUsed = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

	// Update is called once per frame
	void Update () {

        if (player.transform.position.x >= transform.position.x && !alreadyUsed)
        {
            player.GetComponent<Player>().currentCheckpoint = gameObject;
            alreadyUsed = true;
        }

        if (player.GetComponent<Player>().currentCheckpoint != gameObject && alreadyUsed)
            Destroy(gameObject);

	}
}
