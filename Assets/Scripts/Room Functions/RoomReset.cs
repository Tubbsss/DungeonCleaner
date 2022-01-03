using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomReset : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private Transform playerRespawn;

    [SerializeField]
    private Transform roomTransform;

    [SerializeField]
    private List<Pickup> pickups;
    [SerializeField]
    private GameObject pickupPrefab;
    [SerializeField]
    private PickupManager pickupManager;
    [SerializeField]
    private List<Vector3> pickupSpawnPoints;

    [SerializeField]
    private List<LavaDisposal> disposals;

    [SerializeField]
    private List<PressurePlate> pressurePlates;

    [SerializeField]
    private List<DoorController> doors;
    [SerializeField]
    private List<DoorCloseTrigger> doorCloseTriggers;

    void Awake()
    {
        foreach (Pickup p in pickups) //Go through list of pickups in room.
        {
            Vector3 spawnPoint = p.GetComponent<Transform>().position; //Create spawn point at pickup location.
            pickupSpawnPoints.Add(spawnPoint); //Add spawn point to spawn point list.
        }

        //playerRespawn = gameObject.transform; //Set player respawn point to this object's transform.
    }

    private void ResetPlayer()
    {
        player.gameObject.transform.position = playerRespawn.position; //Move player to respawn point.
    }

    private void ResetPickups()
    {
        if (player.pickups.Count > 0) //If player is carrying any pickups.
        {
            for (int i = 0; i < player.pickups.Count; i++) //Go through list of pickups.
            {
                player.pickups[i].transform.parent = roomTransform; //Set parent to room.
                player.pickups.Remove(player.pickups[i]); //Remove pickup from player pickups list.
            }
            player.pickups.Clear(); //Clear list.
            player.sizeMultiplier = 1f; //Reset player size multiplier.
        }

        for (int i = 0; i < pickups.Count; i++) //Go through list of pickups.
        {
            if (pickups[i] != null)
            {
                Destroy(pickups[i].gameObject); //Destroy pickup.
            }
            if (pickups[i] = null)
            {
                pickups.Remove(pickups[i]); //Remove from list.
            }
        }

        pickups.Clear(); //Clear list.

        foreach (Vector3 v in pickupSpawnPoints) //For each spawn point in list.
        {
            Quaternion rotation = new Quaternion(0, 0, 0, 0); //Reset rotation.
            GameObject newPickup = Instantiate(pickupPrefab, v, rotation, roomTransform); //Create new pickup at spawn point.
            newPickup.GetComponent<Pickup>().playerController = player; //Set player controller.
            pickups.Add(newPickup.GetComponent<Pickup>()); //Add new pickup to pickup list.
        }

        pickupManager.pickupsInScene.Clear(); //Clear lsit.
        pickupManager.FindPickupsInScene(); //Run FindPickups function to add new pickups to list.
        pickupManager.RandomisePickupMeshes(); //Randomise meshes again.
        pickupManager.RandomisePickupRotation(); //Randomise rotation.
    }

    private void ResetDisposals()
    {
        foreach (LavaDisposal d in disposals) //For each disposal in room.
        {
            d.allItemsDisposed = false; //Reset all items disposed bool.
            d.itemsToDispose.Clear(); //Clear items to dispose list.
            foreach (Pickup p in pickups)
            {
                d.itemsToDispose.Add(p.gameObject); //Add new pickups to items to dispose list.
            }
            d.itemsRemaining = d.itemsToDispose.Count; //Set items remaining to new total.
        }
    }

    private void ResetPressurePlates()
    {
        foreach (PressurePlate pp in pressurePlates) //For each pressure plate in room.
        {
            pp.isTriggered = false; //Reset trigger.
            pp.transform.localScale = new Vector3(pp.startScale.x, pp.startScale.y, pp.startScale.z); //Reset local scale.
            pp.sizeText.enabled = true; //Re-enable size text.
        }
    }

    private void ResetDoors()
    {
        foreach (DoorController d in doors) //For each door in room.
        {
            d.animator.SetBool("DoorOpen", false); //Close door.
            d.boxCollider.enabled = true; //Enable collider.
            d.doorActive = true; //Set door to active.
        }
        foreach (DoorCloseTrigger dt in doorCloseTriggers) //For each door close trigger in room.
        {
            dt.triggerActive = true; //Enable trigger.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") //When player collides.
        {
            ResetPickups();
            ResetDisposals();
            ResetPressurePlates();
            ResetDoors();
            ResetPlayer();
        }
    }
}
