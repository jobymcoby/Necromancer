using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        //Current camera's position
        Vector3 temp = transform.position;
        // Get 2d Position, manitian camera z (distance)
        temp = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
        //Temp back to camera position
        transform.position = temp;
    }
}
