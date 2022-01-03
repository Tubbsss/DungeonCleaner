using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenTrigger : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private BoxCollider boxCollider;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private DoorController doorController;
    [SerializeField]
    private PlayerController player;

    public bool triggerActive = true;

    private bool playerInCollider = false;

    void Awake()
    {
        triggerActive = true;
    }

    private void Update()
    {
        if (doorController.enabled) //If door controller script active, disable this trigger.
        {
            triggerActive = false;
        }

        if (triggerActive) //If trigger enabled
        {
            if (playerInCollider) //If player is in range.
            {
                if (player.sizeMultiplier > 1) //If the player is carrying any objects.
                {
                    if (animator.GetBool("DoorOpen") != false) //If the door is not already closed.
                    {

                        if (animator != null)
                        {
                            animator.SetBool("DoorOpen", false); //Close door.
                        }

                        if (boxCollider != null)
                        {
                            boxCollider.enabled = true; //Enable collider.
                        }

                        if (audioSource != null)
                        {
                            Debug.Log("Creak");
                            audioSource.Play(); //Play sound fx.
                        }
                    }
                }
                else if (player.sizeMultiplier <= 1) //If player isn't carrying any objects.
                {
                    if (animator.GetBool("DoorOpen") != true) //If door isn't already open.
                    {
                        if (animator != null)
                        {
                            animator.SetBool("DoorOpen", true); //Open door.
                        }

                        if (boxCollider != null)
                        {
                            boxCollider.enabled = false; //Disable collider.
                        }

                        if (audioSource != null)
                        {
                            Debug.Log("Creak");
                            audioSource.Play(); //Play sound fx.
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInCollider = true; //If player in range.
        }
    }
}
