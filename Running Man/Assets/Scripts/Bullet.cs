using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float bulletSpeed = 100;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 0.25f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Time.deltaTime * bulletSpeed, 0, 0);
	}
}
