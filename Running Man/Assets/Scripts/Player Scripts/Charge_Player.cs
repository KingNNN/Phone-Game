using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge_Player : Player {

    // Use this for initialization
    void Start()
    {
        Init();
        myType = PlayerType.BRICK_BERAKER;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCharging)
            Charge();

        movePlayer();

#if (UNITY_EDITOR)
        ButtonPresses(null);
#endif       
    }
}
