using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour
{
    private Toggle toggle;

    [SerializeField]
    private MainMenuController mainMenu;

    [SerializeField]
    private Text costText;

    [SerializeField]
    private Image costSprite;

    [SerializeField]
    private int starCost;

    private int playerStars;

    [SerializeField]
    private string skinName;

    private bool skinUnlocked = false;

    void Awake()
    {
        //Get components.
        toggle = GetComponent<Toggle>();
        mainMenu = GetComponentInParent<MainMenuController>();

        costText = GetComponentInChildren<Text>();
        costText.text = "" + starCost; //Set text to star cost value.

        playerStars = PlayerPrefs.GetInt("Stars"); //Get player stars total from PlayerPrefs.

        if (skinUnlocked)
        {
            costText.text = ""; //Clear text.
        }

        toggle.isOn = false; //Disable toggle.

        if (PlayerPrefs.GetString("SlimeSkin") == skinName) //If skin selected.
        {
            toggle.isOn = true; //Enable toggle.
        }
        else
        {
            toggle.isOn = false; //Disable toggle.
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("Skin" + skinName) == 1) //If skin unlocked in PlayerPrefs.
        {
            skinUnlocked = true; //Unlock skin.
        }
        else
        {
            skinUnlocked = false; //Lock skin.
            PlayerPrefs.SetInt("Skin" + skinName, 0); //Set skin as locked in PlayerPrefs.
        }

        if (skinUnlocked) //If skin unlocked.
        {
            costText.text = ""; //Clear text.
            costSprite.enabled = false; //Disable star icon.
        }
        else
        {
            costText.text = "" + starCost; //Set text.
            costSprite.enabled = true; //Enable star icon.
        }

        

        if (playerStars < starCost) //If player doesn't have enough stars.
        {
            if (!skinUnlocked) //And skin is not unlocked.
            {
                toggle.interactable = false; //Disable interactable.
            }
            if (skinUnlocked) //And skin is unlocked.
            {
                toggle.interactable = true; //Enable interactable.
            }
        }
        else
        {
            toggle.interactable = true; //Enable interactable.
        }
    }

    public void SetPlayerSkin()
    {
        if (!skinUnlocked) //If skin isn't unlocked.
        {
            if (playerStars >= starCost) //And player has enough stars.
            {
                playerStars -= starCost; //Subtract cost from stars total.

                PlayerPrefs.SetInt("Stars", playerStars); //Set stars total in PlayerPrefs.

                skinUnlocked = true; //Unlock skin.

                costText.text = ""; //Clear text.

                PlayerPrefs.SetString("SlimeSkin", skinName); //Set current skin to skin.

                PlayerPrefs.SetInt("Skin" + skinName, 1); //Set star unlocked in PlayerPrefs.
            }
        }
        else if (skinUnlocked) //If skin unlocked.
        {
            costText.text = ""; //Clear text.

            PlayerPrefs.SetString("SlimeSkin", skinName); //Set current skin to skin.
        }
    }
}
