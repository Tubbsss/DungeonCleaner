using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomAI : MonoBehaviour
{
    [SerializeField]
    private List<EnemyAI> roomEnemyAIs;
    [SerializeField]
    private List<RatAI> roomRatAIs;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") //If player enters room.
        {
            for (int i = 0; i < roomEnemyAIs.Count; i++) //Go through list of enemies in room.
            {
                if (roomEnemyAIs[i] != null)
                {
                    roomEnemyAIs[i].aiEnabled = true; //Enable AI.
                    roomEnemyAIs[i].agent.enabled = true; //Enable navmesh agent.
                }
            }
            for (int i = 0; i < roomRatAIs.Count; i++) //Go through list of rats in room.
            {
                if (roomRatAIs[i] != null)
                {
                    if (!roomRatAIs[i].isDead) //If the rat isn't dead.
                    {
                        roomRatAIs[i].aiEnabled = true; //Enable AI.
                        roomRatAIs[i].agent.enabled = true; //Enable navmesh agent.
                    }
                }
            }
        }
    }
}
