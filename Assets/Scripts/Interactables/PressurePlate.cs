using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressurePlate : MonoBehaviour
{
    public bool isTriggered = false;

    [SerializeField]
    private float requiredSizeMultiplier;

    [SerializeField]
    public Text sizeText;

    [SerializeField]
    private PlayerController player;

    public Vector3 startScale;

    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        startScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z); //Store scale.

        float weightNeeded = (requiredSizeMultiplier - 1f) * 10; //Set weight needed to trigger based on size multiplier.

        sizeText.text = "" + (weightNeeded); //Set text to show weight needed.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") //If player on pressure plate.
        {
            if (!isTriggered) //If not already triggered.
            {
                if (player.sizeMultiplier >= requiredSizeMultiplier) //If player carrying enough objects.
                {
                    isTriggered = true; //Trigger.
                    transform.localScale = new Vector3(startScale.x, 0.1f, startScale.z); //Set y scale to 0.1f, so pressure plate looks 'pressed'.
                    sizeText.enabled = false; //Disable text.
                    audioSource.Play(); //Play sound fx.
                }
            }
        }
    }
}
