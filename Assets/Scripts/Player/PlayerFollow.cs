using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform playerTransform;

    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = transform.position - playerTransform.position; //Set camera offset.
    }

    // LateUpdate is called after Update methods
    void LateUpdate()
    {
        Vector3 newPos = playerTransform.position + _cameraOffset; //Move camera to new position.

        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
    }
}
