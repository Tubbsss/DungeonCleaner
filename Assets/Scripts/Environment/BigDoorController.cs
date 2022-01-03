using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDoorController : MonoBehaviour
{
    private Animator animator;

    private BoxCollider boxCollider;

    private bool doorActive = true;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private PressurePlate pressurePlate1;

    [SerializeField]
    private PressurePlate pressurePlate2;

    void Awake()
    {
        //Assign components
        animator = gameObject.GetComponent<Animator>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (doorActive)
        {
            if (pressurePlate1.isTriggered && pressurePlate2.isTriggered)
            {
                OpenDoor();
            }
        }
    }

    private void OpenDoor()
    {
        doorActive = false;

        if (animator != null)
        {
            animator.SetBool("DoorOpen", true);
        }

        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }

        Debug.Log("Door opened!");
    }
}
