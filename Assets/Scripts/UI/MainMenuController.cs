using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public int playerStars = 0;
    [SerializeField]
    private Text playerStarsText;

    //Skin Variables
    public string playerSkinID;
    //Skins Unlocked Bools
    public bool skinDefault = true;
    public bool skinBlue = false;
    public bool skinGreen = false;
    public bool skinRed = false;
    public bool skinRainbow = false;

    void Awake()
    {
        GetAndSetPlayerPrefs();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("delete")) //For debugging.
        {
            ResetPlayerPrefs();
        }
        if (Input.GetKeyUp("up"))
        {
            int currentStars = PlayerPrefs.GetInt("Stars"); //For debugging.
            currentStars++;
            PlayerPrefs.SetInt("Stars", currentStars);
        }

        playerStarsText.text = "" + PlayerPrefs.GetInt("Stars"); //Set player stars text to number of stars.
    }

    public void GetAndSetPlayerPrefs()
    {
        //Set Stars
        if (PlayerPrefs.GetInt("Stars", 0) != 0) //If stars total is not default.
        {
            playerStars = PlayerPrefs.GetInt("Stars"); //Get number of stars.
            PlayerPrefs.SetInt("Stars", playerStars);
        }
        else if (PlayerPrefs.GetInt("Stars", 0) == 0)//Otherwise
        {
            PlayerPrefs.SetInt("Stars", 0); //Set stars total to 0.
            playerStars = PlayerPrefs.GetInt("Stars"); //Get number of stars.
        }
        playerStarsText.text = "" + playerStars; //Update stars text.


        //Set Player Skin
        if (PlayerPrefs.GetString("SlimeSkin", "Default") != "Default") //If skin ID is not default.
        {
            playerSkinID = PlayerPrefs.GetString("SlimeSkin"); //Get skin ID.
            PlayerPrefs.SetString("SlimeSkin", playerSkinID); //Set skin ID to player skin ID.
        }
        else if (PlayerPrefs.GetString("SlimeSkin", "Default") == "Default")//Otherwise
        {
            PlayerPrefs.SetString("SlimeSkin", "Default"); //Set skin ID to default.
            playerSkinID = PlayerPrefs.GetString("SlimeSkin"); //Get skin ID.
        }

        //Set Skins Unlocked
            //Default Skin
            PlayerPrefs.SetInt("SkinDefault", 1); //Always set default skin to unlocked.
            skinDefault = true;
            //Blue Skin
            if (PlayerPrefs.GetInt("SkinBlue", 0) != 0) //If value not 0.
            {
                PlayerPrefs.SetInt("SkinBlue", 1);
                skinBlue = true; //Set skin to unlocked.
            }
            else if (PlayerPrefs.GetInt("SkinBlue", 0) == 0)
            {
                PlayerPrefs.GetInt("SkinBlue", 0); //Set value to 0.
                skinBlue = false; //Set skin to locked.
            }
            //Green Skin
            if (PlayerPrefs.GetInt("SkinGreen", 0) != 0) //If value not 0.
            {
                PlayerPrefs.SetInt("SkinGreen", 1);
                skinGreen = true; //Set skin to unlocked.
            }
            else if (PlayerPrefs.GetInt("SkinGreen", 0) == 0)
            {
                PlayerPrefs.GetInt("SkinGreen", 0); //Set value to 0.
                skinGreen = false; //Set skin to locked.
            }
            //Red Skin
            if (PlayerPrefs.GetInt("SkinRed", 0) != 0) //If value not 0.
            {
                PlayerPrefs.SetInt("SkinRed", 1);
                skinRed = true; //Set skin to unlocked.
            }
            else if (PlayerPrefs.GetInt("SkinRed", 0) != 0)
            {
                PlayerPrefs.GetInt("SkinRed", 0); //Set value to 0.
                skinRed = false; //Set skin to locked.
            }
            //Rainbow Skin
            if (PlayerPrefs.GetInt("SkinRainbow", 0) != 0) //If value not 0.
            {
                PlayerPrefs.SetInt("SkinRainbow", 1);
                skinRainbow = true; //Set skin to unlocked.
            }
            else if (PlayerPrefs.GetInt("SkinRainbow", 0) == 0)
        {
                PlayerPrefs.GetInt("SkinRainbow", 0); //Set value to 0.
                skinRainbow = false; //Set skin to locked.
            }

        PlayerPrefs.Save();
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.SetInt("Stars", 0); //Set stars total to 0.
        playerStarsText.text = "Stars: " + playerStars; //Update stars text.

        PlayerPrefs.SetString("SlimeSkin", "Default"); //Set skin to default.
        playerSkinID = PlayerPrefs.GetString("SlimeSkin");

        PlayerPrefs.SetInt("SkinBlue", 0); //Lock skin.
        skinBlue = false;

        PlayerPrefs.SetInt("SkinGreen", 0); //Lock skin.
        skinGreen = false;

        PlayerPrefs.SetInt("SkinRed", 0); //Lock skin.
        skinRed = false;

        PlayerPrefs.SetInt("SkinRainbow", 0); //Lock skin.
        skinRainbow = false;

        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs Reset");
    }

    public void LoadLevel(string levelName)
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    public void SetPlayerSkin(string skinName)
    {
        playerSkinID = skinName;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
