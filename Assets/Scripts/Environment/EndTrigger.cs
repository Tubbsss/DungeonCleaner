using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTrigger : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private StatController statController;
    [SerializeField]
    private GameObject levelCompletePanel;
    [SerializeField]
    private GameObject timeStatPanel;
    [SerializeField]
    private GameObject cleanStatPanel;
    [SerializeField]
    private GameObject ratStatPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") //If player in range.
        {
            player.inputEnabled = false; //Disable player input.

            statController.trackTime = false; //Stop timer.

            levelCompletePanel.SetActive(true); //Enable level complete UI.

            SetTimeStat();
            SetCleanStat();
            SetRatStat();
        }
    }

    private void SetTimeStat()
    {
        Text timeStatText = timeStatPanel.GetComponentInChildren<Text>(); //Get text component.
        timeStatText.text = "Complete the level in under " + statController.timeTarget + " seconds."; //Set text.

        Toggle timeStatToggle = timeStatPanel.GetComponentInChildren<Toggle>(); //Get toggle component.

        if (statController.BeatTimeTarget()) //If player completed level in time.
        {
            timeStatToggle.isOn = true; //Turn on toggle.
            RewardPlayerStar(); //Give the player a star.
        }
        else
        {
            timeStatToggle.isOn = false; //Turn off toggle.
        }
    }

    private void SetCleanStat()
    {
        statController.CalculateCleanTotal(); //Calculate how clean all cleanable objects are.

        Text cleanStatText = cleanStatPanel.GetComponentInChildren<Text>(); //Get text component.
        cleanStatText.text = "Clean all floors in the level."; //Set text/

        Toggle cleanStatToggle = cleanStatPanel.GetComponentInChildren<Toggle>(); //Get toggle component.

        if (statController.MetCleanTarget()) //If player has cleaned within target threshold.
        {
            cleanStatToggle.isOn = true; //Turn toggle on.
            RewardPlayerStar(); //Give player a star.
        }
        else
        {
            cleanStatToggle.isOn = false; //Turn off toggle.
        }
    }

    private void SetRatStat()
    {
        statController.CheckRats(); //Check if player killed all rats in level.

        Text ratStatText = ratStatPanel.GetComponentInChildren<Text>(); //Get text component.
        ratStatText.text = "Kill all rats in the level."; //Set text component.

        Toggle ratStatToggle = ratStatPanel.GetComponentInChildren<Toggle>(); //Get toggle component.

        if (statController.ratsKilled) //Player killed all rats.
        {
            ratStatToggle.isOn = true; //Turn on toggle.
            RewardPlayerStar(); //Give player a star.
        }
        else
        {
            ratStatToggle.isOn = false; //Turn off toggle.
        }
    }

    private void RewardPlayerStar() //Give player a star.
    {
        int currentStars = PlayerPrefs.GetInt("Stars"); //Get player's current stars.
        currentStars++; //Increase current stars by 1.
        PlayerPrefs.SetInt("Stars", currentStars); //Set new star total.
    }

    private void SavePlayerPrefs()
    {
        PlayerPrefs.Save();
    }
}
