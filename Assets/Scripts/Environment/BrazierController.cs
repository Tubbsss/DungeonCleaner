using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazierController : MonoBehaviour
{
    private bool brazierActive = false;

    private Light brazierLight;

    private ParticleSystem brazierEmitter;

    [SerializeField]
    private PressurePlate pressurePlate;

    private void Awake()
    {
        //Assign Components
        brazierLight = GetComponentInChildren<Light>();
        brazierEmitter = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!brazierActive)
        {
            if (pressurePlate.isTriggered)
            {
                //Light brazier
                brazierActive = true;
                brazierLight.intensity = 4;
                brazierEmitter.Play();
            }
        }
        if (brazierActive)
        {
            if (!pressurePlate.isTriggered)
            {
                //Put out brazier
                brazierActive = false;
                brazierLight.intensity = 0;
                brazierEmitter.Stop();
            }
        }
    }
}
