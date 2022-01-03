using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool inputEnabled = true;
    
    public float speed = 6f;

    public float projectileSpeed = 10.0f;

    public List<GameObject> pickups;

    public float sizeMultiplier = 1f;

    public float pickupSizeMultiplier = 1f;

    private bool playerAtDisposal = false;

    public bool waitingForDisposal = true;

    [SerializeField]
    private GameObject junkShooter; //Game object that pickups are fired from.

    [SerializeField]
    private GameObject playerMesh;

    [SerializeField]
    private BoxCollider playerCollider;

    [SerializeField]
    private BoxCollider playerTriggerCollider;

    [SerializeField]
    private Rigidbody r;

    [SerializeField]
    public AudioSource audioSource;

    [SerializeField]
    public AudioClip moveAudio;

    [SerializeField]
    public AudioClip pickupAudio;

    [SerializeField]
    public AudioClip dropAudio;

    // Update is called once per frame
    void Update()
    {
        if (inputEnabled)
        {
            //Get player input.
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 targetVector = new Vector3(horizontal, 0f, vertical).normalized;

            if (targetVector.magnitude >= 0.1) //Prevent accidental input.
            {
                r.velocity = (targetVector * speed);

                if (audioSource != null)
                {
                    if (moveAudio != null)
                    {
                        if (!audioSource.isPlaying)
                        {
                            audioSource.clip = moveAudio;
                            audioSource.Play();

                            Debug.Log(audioSource.clip);
                        }
                    }
                }
            }

            playerMesh.transform.localScale = new Vector3(sizeMultiplier, sizeMultiplier, sizeMultiplier); //Set player mesh scale to size multiplier.

            playerCollider.size = new Vector3(sizeMultiplier, sizeMultiplier, sizeMultiplier); //Set player collider to size multiplier.
            playerCollider.center = new Vector3(0, ((sizeMultiplier - 1f) / 2), 0); //Adjust centre of collider.

            float triggerSize = sizeMultiplier + 0.1f;

            playerTriggerCollider.size = new Vector3(triggerSize, triggerSize, triggerSize); //Set trigger collider size.
            playerTriggerCollider.center = new Vector3(0, ((triggerSize - 1f) / 2), 0); //Adjust centre of trigger collider.

            pickupSizeMultiplier = 1f - (sizeMultiplier - 1f); //Set pickup size multiplier.

            if (pickups.Count > 0) //If player is carrying pickup.
            {
                CursorController.instance.ActivateCrosshairs(); //Set cursor to crosshair.
            }
            else if (pickups.Count <= 0) //If player isn't carrying pickup.
            {
                CursorController.instance.ClearCursor(); //Reset cursor.
            }

            if (Input.GetButtonDown("Fire1")) //Get fire input.
            {
                if (pickups.Count > 0) //If play is carrying any pickups.
                {
                    int randomIndex = Random.Range(0, pickups.Count); //Randomly select a pickup from list.

                    GameObject projectile = pickups[randomIndex]; //Set that pickup as the projectile.

                    pickups.Remove(projectile); //Remove from pickup list.

                    projectile.transform.parent = junkShooter.transform; // Set parent to junk shooter.
                    projectile.transform.localPosition = new Vector3(0, 0, 1f); //Match position to junk shooter.
                    //projectile.transform.rotation = Quaternion.Euler(0, 0, 0);

                    //Get mouse position.
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Plane plane = new Plane(Vector3.up, Vector3.zero);
                    float distance;
                    if (plane.Raycast(ray, out distance))
                    {
                        //Get fire direction.
                        Vector3 target = ray.GetPoint(distance);
                        Vector3 fireDirection = target - transform.position;
                        fireDirection.y = 0f;

                        projectile.GetComponent<Pickup>().isPickedUp = false; //Set to not be picked up.
                        projectile.GetComponent<Rigidbody>().isKinematic = false; //Enable rigidbody physics.
                        projectile.GetComponent<Rigidbody>().detectCollisions = true; //Enable rigidbody collision.
                        projectile.GetComponent<Rigidbody>().AddForce(fireDirection * projectileSpeed); //Fire object.
                        projectile.transform.parent = null; //Remove parent.
                        projectile.GetComponent<Pickup>().sizeMultiplier = 1f; //Reset pickup size multiplier.

                        if (audioSource != null)
                        {
                            if (dropAudio != null)
                            {
                                audioSource.pitch = Random.Range(0.5f, 1.5f); //Randomise audio pitch.

                                if (!audioSource.isPlaying) //If audio isn't already playing.
                                {
                                    audioSource.clip = dropAudio; //Set audio clip to drop audio.
                                    audioSource.Play(); //Play sound fx.
                                    //Debug.Log(audioSource.clip);
                                }
                                else if (audioSource.isPlaying) //If audio is already playing.
                                {
                                    audioSource.Stop(); //Stop playing audio.
                                    audioSource.clip = dropAudio; //Set audio clip to drop audio.
                                    audioSource.Play(); //Play sound fx.
                                    //Debug.Log(audioSource.clip);
                                }
                            }
                        }

                        sizeMultiplier += -0.1f; //Reduce player size multiplier by 0.1.

                        StartCoroutine(projectile.GetComponent<Pickup>().ResetPickup(1)); //Wait 1 second before reseting the pickup.

                        projectile = null; //Set projectile to null.

                    }
                }
            }
        }
    }

    private void DisposeOfPickup(GameObject pickup, Collider disposalCollider)
    {
        pickup.GetComponent<Pickup>().isDisposed = true; //Set pickup to disposed.
        pickup.transform.parent = disposalCollider.transform; //Set pickup parent to disposal.
        pickup.transform.localPosition = new Vector3(0, 2f, 0); //Set position to above disposal.
        pickup.transform.rotation = Quaternion.Euler(0, 0, 0); //Reset rotation.
        pickup.GetComponent<Rigidbody>().isKinematic = false; //Enable rigidbody physics.
        pickup.GetComponent<Rigidbody>().detectCollisions = true; //Enable rigidbody collision.
    }
}
