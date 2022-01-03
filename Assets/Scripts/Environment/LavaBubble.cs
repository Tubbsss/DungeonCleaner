using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBubble : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private float startDelayTime = 1f;
    [SerializeField]
    private float delayTime;

    void Awake()
    {
        delayTime = startDelayTime; //Set delay time.
    }

    // Update is called once per frame
    void Update()
    {
        if (delayTime > 0) //Countdown timer.
        {
            delayTime -= Time.deltaTime;
        }
        else if (delayTime <= 0) //If delay time is 0.
        {
            delayTime = Random.Range(1, 10); //Set delay time to between 1 & 10 seconds.
            if (audioSource != null)
            {
                if (!audioSource.isPlaying) //If sound not already playing/
                {
                    audioSource.pitch = Random.Range(0.8f, 1.2f); //Set random pitch.
                    audioSource.Play(); //Play sound fx.
                }
            }
        }
    }
}
