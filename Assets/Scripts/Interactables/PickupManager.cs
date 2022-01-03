using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    [SerializeField]
    private List<Mesh> pickupMeshes;

    [SerializeField]
    public List<Pickup> pickupsInScene;

    // Start is called before the first frame update
    void Start()
    {
        FindPickupsInScene(); //Find pickups.

        RandomisePickupMeshes(); //Randomise pickup meshes.

        RandomisePickupRotation(); //Randomise rotation.
    }
    
    public void FindPickupsInScene()
    {
        pickupsInScene.Clear(); //Clear list.

        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Pickup"); //Get all objects tagged as Pickups.
        foreach (GameObject go in gos) //Go through each object in array.
        {
            Pickup pickup = go.GetComponent<Pickup>(); //Get pickup component.
            pickupsInScene.Add(pickup); //Add pickup component to list.
        }
    }

    public void RandomisePickupMeshes()
    {
        foreach (Pickup p in pickupsInScene) //Go through each pickup in list.
        {
            MeshFilter meshFilter = p.GetComponent<MeshFilter>(); //Get mesh filter component.
            MeshCollider meshCollider = p.GetComponent<MeshCollider>(); //Get mesh collider component.

            int randomIndex = Random.Range(0, pickupMeshes.Count); //Randomise index in mesh list.

            meshFilter.mesh = pickupMeshes[randomIndex]; //Set mesh to new random mesh.
            meshCollider.sharedMesh = pickupMeshes[randomIndex]; //Set mesh collider to new random mesh.
        }
    }

    public void RandomisePickupRotation()
    {
        foreach (Pickup p in pickupsInScene) //Go through each pickup in list.
        {
            Transform startPos = p.transform; //Get position.

            p.transform.position = new Vector3(startPos.position.x, 2f, startPos.position.z); //Move object away from floor to prevent getting stuck.

            p.transform.rotation = Random.rotation; //Randomise rotation.
        }
    }
}
