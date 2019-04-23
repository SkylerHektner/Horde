using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{

    public GameObject FinalScene;
    public GameObject CreditsMenu;
    public GameObject UI;

    GameObject Player;
    GameObject Camera;

    float CutsceneDuration = 40f;
    float CreditsDuration = 60.23f;

    bool isPlayingCutscene;
    bool isPlayingCredits;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        //cutscene stuff
        if (isPlayingCutscene)
        {
            CutsceneDuration -= Time.smoothDeltaTime;
            UI.SetActive(false);
        }
        if (CutsceneDuration <= 0) // when cutscene ends, hide the cutscene and roll credits
        {
            CutsceneDuration = 0;
            CreditsMenu.SetActive(true);
            FinalScene.SetActive(false);
            isPlayingCutscene = false;

            isPlayingCredits = true; // begin credits
        }

        //credits stuff
        if (isPlayingCredits)
        {
            CreditsDuration -= Time.smoothDeltaTime;
        }
        if (CreditsDuration <= 0) // credits finish, go back to main menu
        {
            CreditsDuration = 0;
            SceneManager.LoadScene("BetterMenu");
        }
    }

    private void OnTriggerEnter(Collider playa)
    {
        Player.SetActive(false);
        //Camera.SetActive(false);
        FinalScene.SetActive(true);
        isPlayingCutscene = true;
    }
}
