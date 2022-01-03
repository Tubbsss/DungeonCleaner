using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCleanables : MonoBehaviour
{
    [SerializeField]
    private List<CleanableObject> roomCleanables;

    [SerializeField]
    private bool playerInRoom = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") //If player in room.
        {
            for (int i = 0; i < roomCleanables.Count; i++) //Go through list of cleanable objects.
            {
                playerInRoom = true; //Set player in room to true.
                roomCleanables[i].enabled = true; //Enable room cleanable component.
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            for (int i = 0; i < roomCleanables.Count; i++)
            {
                playerInRoom = false; //Set player in room to false.
                roomCleanables[i].enabled = false; //Disable room cleanable component.
            }
        }
    }
}
