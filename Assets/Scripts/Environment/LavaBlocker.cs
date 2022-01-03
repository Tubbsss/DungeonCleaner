using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBlocker : MonoBehaviour
{
    [SerializeField]
    private PressurePlate pressurePlate;

    [SerializeField]
    private LavaDisposal lavaDisposal;

    void Awake()
    {
        lavaDisposal.enabled = false; //Disable to prevent objects getting accidentally disposed.
    }

    // Update is called once per frame
    void Update()
    {
        if (pressurePlate.isTriggered) //If pressure plate has been triggered.
        {
            lavaDisposal.enabled = true; //Enable to allow disposal again.
            Destroy(gameObject); //Destroy self.
        }    
    }
}
