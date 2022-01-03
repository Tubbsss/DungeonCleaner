using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===============================================================================================================
// Early version of disposal, later replaced with LavaDisposal script.
//===============================================================================================================


public class Disposal : MonoBehaviour
{
    public bool allItemsDisposed = false;

    private int itemsRemaining = 0;

    [SerializeField]
    private List<GameObject> itemsToDispose;

    private GameObject player;

    [SerializeField]
    private PlayerController playerController;

    private bool playerInRange = false;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //Get player object.

        itemsRemaining = itemsToDispose.Count; //Set items remaining total.
    }

    // Update is called once per frame
    void Update()
    {
        if (!allItemsDisposed)  //If items stil remain in room.
        {
            if (itemsRemaining <= 0) //If no items remain.
            {
                allItemsDisposed = true;
            }

            if (playerInRange) //If player in range.
            {
                if (Input.GetButton("Interact")) //If player presses interact key.
                {
                    RemoveItemsFromList(); //Remove items from list.
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void RemoveItemsFromList()
    {       
        if (itemsToDispose.Count > 0) //If items in disposal list.
        {
            if (playerController.pickups.Count > 0) //If items in player pickups list.
            {
                foreach (GameObject p in playerController.pickups)
                {
                    itemsToDispose.Remove(p); //Remove item from list.
                    itemsRemaining--; //Reduce item count.
                }
                Debug.Log("All items removed from disposal list");
                playerController.waitingForDisposal = false;
            }
        }

    }
}
