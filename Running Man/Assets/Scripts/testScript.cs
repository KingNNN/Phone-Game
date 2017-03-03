using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour {
   
    public Animator myControl;
    public bool STATE_GROUNDED = false;

    // Use this for initialization
    void Start () {
        myControl = gameObject.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector3 downVec = transform.TransformDirection(Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, downVec, out hit, 1.3f))
        {
            if (hit.collider.gameObject.tag == "Ground")
            {
                STATE_GROUNDED = true;
            }
        }
        else
        {
            STATE_GROUNDED = false;
            myControl.SetTrigger("Grounded");
        }
    }

        // Update is called once per frame
        void Update () {
        if (Input.GetKeyDown("f"))
        {            
            myControl.SetTrigger("Slide");
        }

        if (Input.GetKeyDown("t"))
        {
            myControl.SetTrigger("Jump");
        }
    }
}
