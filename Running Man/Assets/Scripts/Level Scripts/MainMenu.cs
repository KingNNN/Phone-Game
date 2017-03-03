﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public Button playButton,
        customButton,
        optionsButton;

    Animator playAnim,
        customAnim,
        optionsAnim;

    //Animation anim;

	// Use this for initialization
	void Start () {
        //anim = GetComponent<Animation>();

        playAnim = playButton.GetComponent<Animator>();
        customAnim = customButton.GetComponent<Animator>();
        optionsAnim = optionsButton.GetComponent<Animator>();
    }

    public void ButtonPresses(string button)
    {
        switch (button)
        {
            case "play":
                {
                    StartCoroutine("Domino", "up");                        
                }
                break;

            case "custom":
                {
                    StartCoroutine("Domino", "down");
                }
                break;

            default:
                break;
        }

#if (UNITY_EDITOR)
        if (Input.GetKeyDown("w"))
        {
            StartCoroutine("Domino", "up");
        }

        if (Input.GetKeyDown("s"))
        {
            StartCoroutine("Domino", "down");
        }
#endif
    }

    IEnumerator Domino(string dir)
    {
        if (dir == "up")
        {
            //Play Button is Pressed
            optionsAnim.Play("Button Slide");
            yield return new WaitForSeconds(0.25f);
            customAnim.Play("Button Slide");
            yield return new WaitForSeconds(0.25f);
            playAnim.Play("Button Slide");
            yield return new WaitForSeconds(0.75f);
            SceneManager.LoadScene("TempScene");
        }
        else
        {
            optionsAnim.Play("Button Slide In");
            yield return new WaitForSeconds(0.25f);
            customAnim.Play("Button Slide In");
            yield return new WaitForSeconds(0.25f);
            playAnim.Play("Button Slide In");
        }
    }

    // Update is called once per frame
    void Update () {

        //Play Animations from bottom up
        ButtonPresses(null);

    }
}
