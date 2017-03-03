using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default_Player : Player {

    public float pushBackFactor = 5;

	// Use this for initialization
	void Start () {
        Init();
        myType = PlayerType.NORMAL;
    }

    //There has to be a better way to do this, but so far, this is all I got
    //Probably going to have to just move the player's vector diagonally in code
    void pushBack()
    {        
        Vector3 startPos = new Vector3
            (transform.position.x - pushBackFactor, transform.position.y, transform.position.z);
        Vector3 downVec = transform.TransformDirection(Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(startPos, downVec, out hit, 5.0f))
        {
            if (hit.collider.gameObject.tag == "Ground")
            {
                float testG = hit.collider.gameObject.transform.position.y + 0.51f;
                transform.position = new Vector3(startPos.x, testG, startPos.z);
            }
        }
    }

    // Update is called once per frame
    void Update () {
        movePlayer();  

        if (Input.GetKeyDown("f"))
            pushBack();
        
#if (UNITY_EDITOR)
        ButtonPresses(null);
#endif       
    }
}
