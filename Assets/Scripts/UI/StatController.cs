using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatController : MonoBehaviour
{
    //Time Stats
    public bool trackTime = false;
    public float timeTarget = 300f;
    public float timeTotal = 0f;
    [SerializeField]
    private Text timeText;

    //Cleaning Stats
    public bool calcCleanable = false;
    public List<CleanableObject> cleanables;
    public float cleanTarget = 5f;
    public float cleanTotal = 0f;

    //Rat Stats
    public List<RatAI> rats;
    public bool ratsKilled = false;

    void Awake()
    {
        timeTotal = 0f; //Reset time total.
        trackTime = true; //Start counter.

        cleanTotal = 0f; //Reset clean total.
        FindCleanables(); //Find cleanables in level.

        FindRats(); //Find rats in level.
    }

    // Update is called once per frame
    void Update()
    {
        if (trackTime) //If timer enabled.
        {
            TrackTime(); //Timer.
        }

        if (calcCleanable)
        {
            CalculateCleanTotal(); //Calculate current cleanliness.
        }
    }

    private void TrackTime()
    {
        timeTotal += Time.deltaTime;

        float minutes = Mathf.FloorToInt(timeTotal / 60);
        float seconds = Mathf.FloorToInt(timeTotal % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public bool BeatTimeTarget()
    {
        timeText.text = ""; //Clear text.

        if (timeTotal > timeTarget) //If player failed to complete level in time.
        {
            return false; //Return false.
        }
        return true; //Return true.
    }

    private void FindCleanables()
    {
        cleanables.Clear(); //Clear list.

        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Cleanable"); //Populate array with cleanable objects.
        foreach (GameObject go in gos)
        {
            CleanableObject cleanable = go.GetComponent<CleanableObject>(); //Get cleanable component.
            cleanables.Add(cleanable); //Add to list.
        }
    }

    public void CalculateCleanTotal()
    {
        float cleanCalc = 0f; //Reset clean value.

        foreach (CleanableObject cleanable in cleanables) //Go through list of cleanables.
        {
            cleanable.enabled = true; //Enable cleanable component.

            cleanCalc += Mathf.RoundToInt(cleanable.GetDirtAmount()); //Get dirt amount (rounded).
            Debug.Log(cleanTotal);

            cleanable.enabled = false; //Disable cleanable component.
        }
        cleanTotal = (cleanCalc / cleanables.Count) * 100f; //Set clean total.

        Debug.Log("Final Clean Total: " + cleanTotal);
        calcCleanable = false;
    }

    public bool MetCleanTarget()
    {
        if (cleanTotal > cleanTarget) //If clean total is greater than target.
        {
            return false; //Return false.
        }
        return true; //Return true.
    }

    private void FindRats()
    {
        rats.Clear(); //Clear list.

        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Rat"); //Populate list with rats in level.
        foreach (GameObject go in gos)
        {
            RatAI rat = go.GetComponent<RatAI>(); //Get rat component.
            rats.Add(rat); //Add to list.
        }
    }

    public void CheckRats()
    {
        for (int i = 0; i < rats.Count; i++) //Go through list of rats.
        {
            if (rats[i].isDead == false) //If any rat isn't dead.
            {
                ratsKilled =  false; //Set ratsKilled to false.
                return;
            }
        }
        ratsKilled = true; //Set ratsKilled to true.
        return;
    }
}
