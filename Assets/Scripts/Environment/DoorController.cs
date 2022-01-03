using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator animator;

    public BoxCollider boxCollider;

    public bool doorActive = true;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private CleanableObject cleanableObject;

    [SerializeField]
    private LavaDisposal disposal;

    [SerializeField]
    private PressurePlate pressurePlate;

    [SerializeField]
    private bool tutDirtClean = false;

    [SerializeField]
    private bool tutJunkDisposal = false;

    [SerializeField]
    private bool tutPressurePlate = false;

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
            if (tutDirtClean) //If opening tied to dirt cleaning.
            {
                TutDirtCleanCheck();
            }
            if (tutJunkDisposal) //If openening tied to disposing of junk.
            {
                TutJunkDisposalCheck();
            }
            if (tutPressurePlate) //If opening tied to pressure plate.
            {
                TutPressurePlateCheck();
            }
        }
    }

    private void OpenDoor()
    {
        doorActive = false;

        if (animator != null)
        {
            animator.SetBool("DoorOpen", true); //Play door open animation.
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

        Debug.Log("Door opened!");
    }

    private void TutDirtCleanCheck()
    {
        if (((cleanableObject.dirtAmount / cleanableObject.dirtAmountTotal) * 100) <= cleanableObject.dirtThreshold) //If cleanable object is cleaned to within threshold.
        {
            OpenDoor();
        }
    }

    private void TutJunkDisposalCheck() //If all items in room have been disposed of.
    {
        if (disposal.allItemsDisposed)
        {
            OpenDoor();
        }
    }

    private void TutPressurePlateCheck() //If pressure plate has been triggered.
    {
        if (pressurePlate.isTriggered)
        {
            OpenDoor();
        }
    }
}
