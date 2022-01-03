using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloseTrigger : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private BoxCollider boxCollider;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private DoorController doorController;

    public bool triggerActive = true;

    [SerializeField]
    private bool startDoorOpen = true;

    void Awake()
    {
        triggerActive = true;

        if (startDoorOpen)
        {
            if (animator != null)
            {
                animator.SetBool("DoorOpen", true);
            }

            if (boxCollider != null)
            {
                boxCollider.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (triggerActive) //If play in room & trigger has not already been triggered.
            {
                triggerActive = false;

                if (animator != null)
                {
                    animator.SetBool("DoorOpen", false); //Play close animation.
                }

                if (boxCollider != null)
                {
                    boxCollider.enabled = true; //Enable collision.
                }

                if (audioSource != null)
                {
                    Debug.Log("Creak");
                    audioSource.Play(); //Play sound fx.
                }

                if (doorController != null)
                {
                    doorController.enabled = true; //Enable door controller script.
                }
            }
        }
    }
}
