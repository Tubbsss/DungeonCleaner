using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaDisposal : MonoBehaviour
{
    public bool allItemsDisposed = false;

    public int itemsRemaining = 0;

    [SerializeField]
    public List<GameObject> itemsToDispose;

    // Start is called before the first frame update
    void Awake()
    {
        itemsRemaining = itemsToDispose.Count; //Set items remaining total.
    }

    // Update is called once per frame
    void Update()
    {
        if (!allItemsDisposed) //If items stil remain in room.
        {
            if (itemsRemaining <= 0) //If no items remain.
            {
                allItemsDisposed = true;
            }
        }
    }

    private void DisposePickup(GameObject pickup)
    {
        if (itemsRemaining != 0) //If there are items left in room.
        {
            itemsRemaining--; //Reduce total.
        }
        Destroy(pickup); //Destroy pickup.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pickup" || other.gameObject.tag == "Rat") //If pickup or rat thrown in disposal
        {
            DisposePickup(other.gameObject);
        }
    }
}
