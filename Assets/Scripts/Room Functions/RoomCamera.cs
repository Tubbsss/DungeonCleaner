using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamera : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private Transform roomCameraPos;

    public bool followPlayer = false;

    private PlayerFollow playerFollow;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main; //Get main camera.
        playerFollow = mainCamera.GetComponent<PlayerFollow>(); //Get player follow component.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") //If player in room.
        {
            if (roomCameraPos != null)
            {
                mainCamera.transform.position = roomCameraPos.position; //Move main camera to new position.

                if (followPlayer)
                {
                    playerFollow.enabled = true; //Enable player follow component.
                }
                else
                {
                    playerFollow.enabled = false; //Disable player follow component.
                }
            }
        }
    }

}
