using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "P_Bullet")
        {
            GameObject.FindGameObjectWithTag("Player").SendMessage("AddScore", 50.0f);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 3.0f);
    }
}
