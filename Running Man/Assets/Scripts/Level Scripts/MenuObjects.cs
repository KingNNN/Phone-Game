using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MenuObjects : MonoBehaviour {

    /*******MAIN MENU************/
    public Button playButton,
        customButton,
        optionsButton;

    public Animator playAnim,
        customAnim,
        optionsAnim;
    /***************************/

    /********GAME MODES**********/
    public Button runGame,
        stackGame,
        targetGame,
        tankGame,
        fanGame;    

    public Animator runnerAnim,
        stackAnim,
        targetAnim,
        tankAnim,
        fanAnim;
    /***************************/

    public void InitMenu()
    {
        //MAIN MENU
        playAnim = playButton.GetComponent<Animator>();
        customAnim = customButton.GetComponent<Animator>();
        optionsAnim = optionsButton.GetComponent<Animator>();

        //GAME MODES
        runnerAnim = runGame.GetComponent<Animator>();
        stackAnim = stackGame.GetComponent<Animator>();
        targetAnim = targetGame.GetComponent<Animator>();
        tankAnim = tankGame.GetComponent<Animator>();
        fanAnim = fanGame.GetComponent<Animator>();
    }
}
