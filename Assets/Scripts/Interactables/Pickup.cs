using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PlayerController playerController;

    public bool isPickupable = true;

    public bool isPickedUp = false;

    public bool isDisposed = false;

    public Material pickupMaterial;

    public float sizeMultiplier = 1f;

    public bool isStatic = true;

    public IEnumerator ResetPickup(int seconds) //Delay isPickupable reset to prevent objects getting stuck.
    {
        int counter = seconds;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }
        isPickupable = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isPickupable == true) //If player in range & object pickupable.
        {
            isPickupable = false; //Set to not pickupable.

            isPickedUp = true; //Set to picked up.

            isStatic = false;

            playerController.pickups.Add(gameObject); //Add to player pickups list.

            playerController.GetComponent<PlayerController>().sizeMultiplier += 0.1f; //Get player sizeMultiplier.

            transform.parent = other.transform; //Set player as parent object.

            //Randomise position within player mesh.
            float randPos = 0.5f + (sizeMultiplier - 1f);

            float randX = Random.Range(-randPos, randPos);
            float randY = Random.Range(0.0f, randPos);
            float randZ = Random.Range(-randPos, randPos);

            Debug.Log("RandX = " + randX);
            Debug.Log("RandY = " + randY);
            Debug.Log("RandZ = " + randZ);

            transform.localPosition = new Vector3(randX, randY, randZ);

            transform.localRotation = Random.rotation;

            //Disable collisions and stop calculating physics.
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Rigidbody>().detectCollisions = false;

            AudioSource audioSource = playerController.GetComponent<AudioSource>(); //Get player audio source.

            if (audioSource != null)
            {
                if (playerController.pickupAudio != null)
                {
                    audioSource.pitch = Random.Range(0.5f, 1.5f); //Randomise audio pitch.

                    if (!audioSource.isPlaying) //If audio source is not playing.
                    {
                        audioSource.clip = playerController.pickupAudio; //Set audio clip.
                        audioSource.Play();
                        Debug.Log(audioSource.clip);
                    }
                    else if (audioSource.isPlaying) //If audio source is playing.
                    {
                        audioSource.Stop(); //Stop playuing audio first.
                        audioSource.clip = playerController.pickupAudio;//Set audio clip.
                        audioSource.Play();
                        Debug.Log(audioSource.clip);
                    }
                }
            }
        }

        if (other.gameObject.tag == "Disposal" && isDisposed == true)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && isPickedUp != true && isStatic != true) //If collide with enemy and not carried by player or hasn't been interacted with yet.
        {
            var enemy = collision.gameObject.GetComponent<EnemyAI>(); //Get enemy AI component
            var pickup = collision.gameObject.GetComponent<Pickup>(); //Get enemy pickup component.

            enemy.aiEnabled = false; //Disable AI.

            pickup.enabled = true; //Enable pickup component.
            pickup.isPickupable = true; //Set pickupable.
        }
        if (collision.gameObject.tag == "Rat" && isPickedUp != true && isStatic != true) //If collide with enemy and not carried by player or hasn't been interacted with yet.
        {
            var rat = collision.gameObject.GetComponent<RatAI>(); //Get rat AI component.
            var pickup = collision.gameObject.GetComponent<Pickup>(); //Get rat pickup component.

            if (!rat.isDead) //If rat isn't already dead.
            {
                rat.aiEnabled = false; //Disable AI.
                if (rat.audioSource != null)
                {
                    rat.audioSource.clip = rat.ratDeath; //Set rat death sound.
                    rat.audioSource.loop = false; //Disable looping.
                    rat.audioSource.Play(); //Play
                }
                rat.isDead = true; //Set rat to dead.

                pickup.enabled = true; //Enable pickup component.
                pickup.isPickupable = true; //Set pickupable.
            }
        }
    }
}
