﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MenuObjects {

	// Use this for initialization
	void Start () {
        InitMenu();
    }

    //Main Menu Navigation
    public void ButtonPresses(string button)
    {
        switch (button)
        {
                /*********************MAIN MENU*******************/
            case "play":
                {
                    StartCoroutine(MAIN_Domino("up", "modes"));                        
                }
                break;

            case "custom":
                {
                    StartCoroutine(MAIN_Domino("down",null));
                }
                break;
                /*************************************************/

                /*******************GAME MODES*******************/
            case "run":
                {
                    SceneManager.LoadScene("TempScene");
                }
                break;

            case "stack":
                {
                    SceneManager.LoadScene("Stacker");
                }
                break;

            case "target":
                {
                    SceneManager.LoadScene("Target Practice");
                }
                break;

            case "tanks":
                {
                    //SceneManager.LoadScene("Target Practice");
                }
                break;
            /************************************************/
            default:
                break;
        }

#if (UNITY_EDITOR)
        if (Input.GetKeyDown("w"))
        {
            StartCoroutine(MAIN_Domino("up", null));
        }

        if (Input.GetKeyDown("s"))
        {
            StartCoroutine(MAIN_Domino("up", null));
        }
#endif
    }

    IEnumerator MAIN_Domino(string dir, string next)
    {
        //Main Menu
        switch (dir)
        {
            //Play Button is Pressed
            case "up":
                {                    
                    //Main Menu leaves
                    optionsAnim.Play("Button Slide");
                    yield return new WaitForSeconds(0.25f);
                    customAnim.Play("Button Slide");
                    yield return new WaitForSeconds(0.25f);
                    playAnim.Play("Button Slide");
                    yield return new WaitForSeconds(0.25f);
                }
                break;

            case "down":
                {
                    optionsAnim.Play("Button Slide In");
                    yield return new WaitForSeconds(0.25f);
                    customAnim.Play("Button Slide In");
                    yield return new WaitForSeconds(0.25f);
                    playAnim.Play("Button Slide In");
                }
                break;

            default:
                break;
        }

        //Play Was Pressed
        switch (dir)
        {
            case "modes":
                {
                    //Game Modes Appear
                    runnerAnim.Play("Button Slide In");
                    yield return new WaitForSeconds(0.25f);
                    stackAnim.Play("Button Slide In");
                    yield return new WaitForSeconds(0.25f);
                    targetAnim.Play("Button Slide In");
                    yield return new WaitForSeconds(0.25f);
                    tankAnim.Play("Button Slide In");
                }
                break;

            default:
                break;
        }
    }

    // Update is called once per frame
    void Update () {

        //Play Animations from bottom up
        ButtonPresses(null);

    }
}
