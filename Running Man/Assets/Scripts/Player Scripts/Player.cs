using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour {

    public enum PlayerType { NORMAL, BRICK_BERAKER, FLOATER, GHOST }

    protected PlayerType myType = PlayerType.NORMAL;   //Player type that restricts certain abilities
                                                    //Only change in child classes when they are created

    public Transform camPos,                     
                     firePoint;

    //Player's Box collider along with it's size and center pos
    BoxCollider myCollider;
    protected float colCY, colCYSlide = 3.5f,
                    colSY, colSYSlide = 9.0f;

    //Seperate from bottom GameObjects because used for different reasons
    public GameObject spawnPoint,
                      currentCheckpoint;

    GameObject bullet,
               lowObs,
               hiObs;

    //Animations and junk
    protected Animator myControl;

    protected float runSpeed,               //Speed of player
                 baseSpeed,                 //Starting speed of player
                 maxSpeed,                  //Max speed of player
                 accelerationRate,          //Keep less than one
                 camDiff,                   //Distance from player to cam
                 jumpHeight,                //Max height if jump
                 jumpSpeedFactor = 8.5f,    //Adjusts speed and snappiness of jump
                 gravityVal,                //Speed of decent from jump
                 hiObsPoint = 0.8f,         //Pos of HI_OBS when random spawning
                 lowObsPoint = -1.06f,      //Pos of LO_OBS when random spawning
                 scoreTimer,                //Timer to add to score
                 scoreCount,                //Time between to add score
                 score = 0,                 //...score, duh...
                 maxSpawnTime,              //To be honest, I don't remember it's used in
                 minSpawnTime,              // SpawnObstacle() and says it's to shorten time between spawns
                 pushBackCount = 0.0f,      //Used when player is hit to push back some
                 pushBackTime = 0.5f;       //Max time to slow down player

    protected Text scoreText;

    protected bool STATE_GROUNDED = false,     //Grounded state
                floatDown = false,          //Flag to allow player to float slower than normal
                isJumping = false,          //Flag that signifies that player is in air
                canDblJump = false,         //Player can double jump if true
                isSliding = false,          //Player can slide if true
                canCharge = true,           //Restricts player from abusing charge in air
                isCharging = false,         //Flag that signifes charging
                pushBackPlayer = false,     //Used when player is hit to push back some
                isFading = false,           //Initiate fading process
                fadeIn = false;             //Used to make player fade back in after fading alpha

    protected float groundLevel,            //Change later to use raycasting
                  jumpVal,                  //Distance from ground to peak and vice versa
                  cameraYPos,               //
                  slideTime = 0.0f,         //Actual sliding time
                  slideTimeMax = 0.375f,      //Max slide time (Distance)
                  chargeTime = 0.0f,        //Actual charge time
                  chargeTimeMax = 0.35f,    //Max charge time (Distance)
                  randomSpawnVal = 0,       //Timer to spawn obs
                  randomSpawnCount;         //Random number between 1 and 2

    protected float myR,            //Vars used to fade in and out in transparency
              myG,
              myB,
              myA,
              fadeTime,             //Has to be changed in the start finc for some reason...chalk it up to unity being unity
              fadeCount = 0.0f;     

    // Use this for initialization
    protected void Init () {

        //myType = PlayerType.NORMAL;
        myControl = gameObject.GetComponent<Animator>();

        spawnPoint = GameObject.Find("Start Point");
        currentCheckpoint = spawnPoint;

        myCollider = gameObject.GetComponent<BoxCollider>();
        colCY = myCollider.center.y;
        colSY = myCollider.size.y;

        //myR = gameObject.GetComponent<MeshRenderer>().material.color.r;
        //myG = gameObject.GetComponent<MeshRenderer>().material.color.g;
        //myB = gameObject.GetComponent<MeshRenderer>().material.color.b;
        //myA = gameObject.GetComponent<MeshRenderer>().material.color.a;

        fadeTime = 0.4f;     

        scoreTimer = 1.0f;
        scoreCount = 0;
        scoreText = GameObject.Find("Score").GetComponent<Text>();

        bullet = (GameObject)Resources.Load("Bullet");

        //Change name when final models come into play
        lowObs = (GameObject)Resources.Load("OBS_low");
        hiObs = (GameObject)Resources.Load("OBS_hi");

        camDiff = 2.36f;

        baseSpeed = 7;
        maxSpeed = 14;
        runSpeed = baseSpeed;

        //Used to add to runSpeed over time
        //Keep below one to make game managable
        accelerationRate = 0.25f;

        jumpHeight = 3.0f;
        groundLevel = transform.position.y;

        jumpVal = jumpHeight;
        gravityVal = jumpHeight * 0.5f;

        cameraYPos = camPos.position.y;

        maxSpawnTime = 2;
        minSpawnTime = 0.6f;

        randomSpawnCount = Random.Range(1, 2.0f);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "OBS_LOW" || other.tag == "OBS_HI")
        {
            if (!pushBackPlayer)
            {
                pushBackPlayer = true;
                runSpeed /= 2;
            }
            //runSpeed = baseSpeed;
        }

        if (other.tag == "Pickup_Coin")
        {
            score += 100;            
            Destroy(other.gameObject);
        }
    }    

    //Called at the end of each frame
    //So far just used for raycasting to the ground and checking for a breakable wall
    void FixedUpdate()
    {
        Vector3 downVec = transform.TransformDirection(Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, downVec, out hit, 1.0f))
        {
            if (hit.collider.gameObject.tag == "Ground")
            {
                STATE_GROUNDED = true;
                myControl.SetBool("Grounded",true);
                myControl.SetBool("Falling", false);
                gravityVal = jumpHeight * 0.5f;//                           probably should make this a var
                groundLevel = hit.collider.gameObject.transform.position.y + 0.51f;

                //if (!isJumping)
                 //   transform.position = new Vector3(transform.position.x, groundLevel, transform.position.z);
            }            
        }
        else
        {
            STATE_GROUNDED = false;
            myControl.SetBool("Grounded", false);
            //myControl.SetTrigger("Grounded");

            //Gravity Implementation
            if (!isJumping)
            {
                canDblJump = true;

                //Play falling animation
                myControl.SetBool("Falling", true);

                float fallSpeedFactor = 6.0f;                

                //Used for jump and fall speed
                float fallSpeed = Time.deltaTime * fallSpeedFactor;

                float posY = transform.position.y - (gravityVal * fallSpeed);

                //Have camera fall with player
                cameraYPos = (transform.position.y + camDiff);

                transform.position = new Vector3(transform.position.x, posY, transform.position.z);
            }
        }

        //Destroys wall in front of player while CHARGING
        if (myType == PlayerType.BRICK_BERAKER)
        {
            if (Physics.Raycast(transform.position, transform.right, out hit, 1.0f))
            {
                if (hit.collider.gameObject.tag == "Breakable Wall")
                {
                    if (isCharging)
                        Destroy(hit.collider.gameObject);
                    else    //Reset Pos
                    {
                        if (!pushBackPlayer)
                        {
                            pushBackPlayer = true;
                            runSpeed /= 2;
                        }
                    }
                }
            }
        }
    }

    protected void AdjustCamerHeight()
    {
        //Adjustment rate makes for a weird bug where the camera jumps when the 
        //player lands in else statement of FixedUpdate. Currently trying to fix
        if (cameraYPos > (groundLevel + camDiff))
            cameraYPos -= Time.deltaTime * 5;

        if (cameraYPos < (groundLevel + camDiff))
            cameraYPos += Time.deltaTime * 5;

        if (cameraYPos >= (groundLevel + camDiff))
            cameraYPos = (groundLevel + camDiff);
    }

    //Adjust the local jumpSpeedFactor var to change the speed of jump
    void Jump()
    {
        float timeD = Time.deltaTime;

        if (isSliding)
        {
            slideTime = 0.0f;
            isSliding = false;
            myCollider.center = new Vector3(myCollider.center.x, colCY, myCollider.center.z);
            myCollider.size = new Vector3(myCollider.size.x, colSY, myCollider.size.z);
        }

        if (!isCharging)
       {
            //Used for jump and fall speed
            float jumpSpeed = timeD * jumpSpeedFactor;

            //Jumping up
            transform.position += new Vector3(0, (jumpVal * jumpSpeed), 0);

            if (myType == PlayerType.FLOATER && floatDown && jumpVal < 0)
                gravityVal = jumpHeight * 0.01f; //Float Gravity
            else
                gravityVal = jumpHeight * 0.5f; //Normal Gravity

            //Gravity effect        
            jumpVal = jumpVal - (gravityVal * jumpSpeed);
       }

        if (isCharging)
            canCharge = false;

        //Reset Jump bool and state
        if (transform.position.y <= groundLevel)
        {
            transform.position = new Vector3(transform.position.x, groundLevel, transform.position.z);
            jumpVal = jumpHeight;
            isJumping = false;
        }
    }

    void Slide()
    {
        float timeD = Time.deltaTime;

        //if (transform.localScale.y >= 1)
        //    transform.localScale = new Vector3(1, 0.5f, 1);

        slideTime += timeD;

        if (slideTime >= slideTimeMax)
        {
            slideTime = 0.0f;
            isSliding = false;

            //Ran into an issue where it lingered on the slide anim so this forces it into the run anim
            if (myControl.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
                myControl.SetTrigger("Get Up");

            myCollider.center = new Vector3(myCollider.center.x, colCY, myCollider.center.z);
            myCollider.size = new Vector3(myCollider.size.x, colSY, myCollider.size.z);
        }

    }

    /**********Special Abilities**************/
    protected void Charge()
    {
        runSpeed = 21;

        chargeTime += Time.deltaTime;

        jumpVal = 0;

        if (chargeTime >= chargeTimeMax)
        {
            runSpeed = baseSpeed;
            chargeTime = 0;
            isCharging = false;

            //Restricts DASH so that player cannot infinitely dash in air
            if (!isJumping)
                jumpVal = jumpHeight;            
        }
    }
    
    void Shoot()
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }

    //Make Stumble anim

    //For this to work, the material has to be set to transparency
    void fadePlayerTransparency()
    {
        //A if measured 0-1
        float minA = 0.05f;
        fadeCount += Time.deltaTime;

        if (fadeCount >= fadeTime)
            fadeIn = true;

        if (!fadeIn)
            myA -= (Time.deltaTime * 5);
        else
            myA += (Time.deltaTime * 5);

        if (myA < minA)
            myA = minA;

        gameObject.GetComponent<MeshRenderer>().material.color =
            new Color(myR, myG, myB, myA);

        if (fadeIn && myA >= 1)
        {
            fadeIn = false;
            isFading = false;
            fadeCount = 0;
            myA = 1;
        }
    }
    /*****************************************/

    //Spawn random obstacles in world so that each play is different
    //Reserved for a different game mode
    void SpawnObstacle()
    {
        int seed = Random.Range(0, 100);
        Vector3 spawnPoint;

        if (seed % 2 == 0)  //LOW
        {
            spawnPoint = new Vector3(transform.position.x + 10, -1.06f, transform.position.z);
            Instantiate(lowObs,spawnPoint,lowObs.transform.rotation);
        }
        else    //HI
        {
            spawnPoint = new Vector3(transform.position.x + 10, 0.8f, transform.position.z);
            Instantiate(hiObs, spawnPoint,hiObs.transform.rotation);
        }

        //Shortens distance between obstacles
        //Don't know if this is final, just messing around with ideas
        maxSpawnTime -= Time.deltaTime * Random.Range(1, 10);
        if (maxSpawnTime <= minSpawnTime)
            maxSpawnTime = minSpawnTime;

        randomSpawnCount = Random.Range(1, maxSpawnTime);
        randomSpawnVal = 0;
    }

    protected void gotoLastCheckpoint()
    {
        Vector3 downVec = transform.TransformDirection(Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(currentCheckpoint.transform.position, downVec, out hit, 5.0f))
        {
            if (hit.collider.gameObject.tag == "Ground")
            {
                groundLevel = hit.collider.gameObject.transform.position.y + 0.51f;
            }
        }

        transform.position = new Vector3(currentCheckpoint.transform.position.x, groundLevel,
                                         currentCheckpoint.transform.position.z);
    }

    public void AddScore(float x) { score += x; }

    //Try to put all button processing in here to clear up update funcf
    public void ButtonPresses(string name)
    {
        //Used for debug and final builds on mobile platform
        switch (name)
        {
            case "duck":
                {
                    if (!isSliding && !isJumping)
                        isSliding = true;
                }
                break;

            //Not sure how to make flote on phone
            //Maybe check if still recieving message on decent?
            case "jump":
                {
                    if (isJumping && canDblJump)
                    {
                        jumpVal = jumpHeight;
                        canDblJump = false;
                    }

                    if (!isJumping)
                    {
                        isJumping = true;
                        canDblJump = true;
                    }
                }
                break;

            case "action":
                {
                    switch (myType)
                    {
                        case PlayerType.NORMAL:
                            {
                                Shoot();
                            }
                            break;

                        case PlayerType.GHOST:
                            {
                                isFading = true;
                            }
                            break;

                        case PlayerType.FLOATER:
                            {
                                //Uncomment when building
                                //I think this checks for prolonged touches
                                //for (int i = 0; i < Input.touchCount; ++i)
                                //{
                                //    if (Input.GetTouch(i).phase == TouchPhase.Stationary)
                                        floatDown = true;
                                //}
                            }
                            break;

                        case PlayerType.BRICK_BERAKER:
                            {
                                isCharging = true;
                            }
                            break;

                        default:
                            break;
                    }
                }
                break;

            default:
                break;
        }

#if(UNITY_EDITOR)
        if (Input.GetKeyDown("w"))
        {
            //Second Jump
            if (!STATE_GROUNDED && canDblJump)
            {
                jumpVal = jumpHeight;                
                canDblJump = false;

                //Resets current animation state
                myControl.Play("Jump",-1,0.0f);                
            }            

            //First Jump
            if (STATE_GROUNDED)
            {
                isJumping = true;
                canDblJump = true;

                if(myControl.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
                    myControl.Play("Jump", -1, 0.0f);

                if (myControl.GetCurrentAnimatorStateInfo(0).IsName("Run"))             
                    myControl.SetTrigger("Jump");
            }
        }

        //Fade Transparancy
        if (Input.GetKey("q") && !isFading && myType == PlayerType.GHOST)
            isFading = true;

        //Floating
        if (Input.GetKey("w") && myType == PlayerType.FLOATER)
            floatDown = true;
        else
            floatDown = false;

        //Sliding
        if (Input.GetKeyDown("s"))
        {
            if (!isSliding && STATE_GROUNDED)
            {
                if (myControl.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    isSliding = true;
                    myCollider.center = new Vector3(myCollider.center.x, colCYSlide, myCollider.center.z);
                    myCollider.size = new Vector3(myCollider.size.x, colSYSlide, myCollider.size.z);

                    myControl.SetTrigger("Slide");
                    //myControl.Play("Slide", -1, 0.0f);                    
                }
            }
        }       

        //Debugging for checkpoints
        if (Input.GetKeyDown("p"))
        {
            gotoLastCheckpoint();
        }

        //Fire weapon
        if (Input.GetKeyDown("space") && myType == PlayerType.NORMAL)
        {
            Shoot();
        }

        //Charge though walls
        if (Input.GetKeyDown("e") && myType == PlayerType.BRICK_BERAKER)
        {
            if (canCharge && !isCharging && !isSliding)
                isCharging = true;
        } 
#endif
    }

    protected void movePlayer()
    {
        //Because I'm lazy
        float timeD = Time.deltaTime;

        //
        //if (STATE_GROUNDED)
        //    canCharge = true;

        //Main Function flags
        if (isJumping)
            Jump();

        if (isSliding)
            Slide();

        //Increase speed over time
        if (!isCharging)
        {
            if (runSpeed < maxSpeed)
                runSpeed += (timeD * accelerationRate);
            if (runSpeed > maxSpeed)
                runSpeed = maxSpeed;
        }

        //Makes camera follow player
        //camPos.position = new Vector3(camPos.position.x, cameraYPos, camPos.position.z);
        //GameObject.Find("Camera").GetComponent<Transform>().position = camPos.position;

        //Slow down player when it hits an object
        if (pushBackPlayer)
        {
            pushBackCount += timeD;

            if (pushBackCount >= pushBackTime)
            {
                pushBackCount = 0.0f;
                runSpeed = baseSpeed;
                pushBackPlayer = false;
            }
        }

        //Constant motion
        //Change when the final models come in
        //transform.Translate(0, 0, (timeD * runSpeed));

        //Adjust Camera for Different Plats                2.36
        if (STATE_GROUNDED && cameraYPos != (groundLevel + camDiff))
            AdjustCamerHeight();
    }

	// Update is called once per frame
	void Update () {
        float timeD = Time.deltaTime;

        //Spawn obstacles in front of the player as it goes
        //Make into own game mode downt the line
        //randomSpawnVal += timeD;
        //if (randomSpawnVal >= randomSpawnCount)
        //    SpawnObstacle();

        movePlayer();

        //Input processing
#if (UNITY_EDITOR)
        ButtonPresses(null);
#endif

        //Main mechanics
        if (isCharging)
            Charge();

        if (isFading)
            fadePlayerTransparency();        

        //Running score and update
        scoreCount += timeD;
        if (scoreCount >= scoreTimer)
        {
            score += 10;
            scoreCount = 0;
        }        
        scoreText.text = "Score: " + score;
    }
}