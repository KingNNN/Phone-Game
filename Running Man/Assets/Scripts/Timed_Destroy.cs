using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timed_Destroy : MonoBehaviour {
    	
	// Update is called once per frame
	void Update () {
        Destroy(gameObject ,3.0f);
	}
}
