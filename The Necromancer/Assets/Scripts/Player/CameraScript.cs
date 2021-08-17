using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Transform playerTransform;
    private Vector3 mousePosition;
    private Vector3 cameraPosition;
    

    private float wX = .28f;
    private float wY = .4f;

    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform.Find("CameraPoint").transform;
        mousePosition = playerTransform.position;
    }

    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cameraPosition = FindMidpoint(mousePosition, playerTransform);
    }

    void LateUpdate()
    {
        //Current camera's position
        Vector3 temp = this.transform.position;

        // if the player and camera are close use player position, else find midpoint
        if (Vector3.Distance(cameraPosition, playerTransform.position) < 1.3f)
        {
            temp = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
        }
        else
        {
            temp = new Vector3(cameraPosition.x, cameraPosition.y, -10);
        }

        //Temp back to camera position
        this.transform.position = Vector3.Lerp(this.transform.position, temp, 1/5f);
    }

    private Vector3 FindMidpoint(Vector3 mousePosition, Transform playerTransform)
    {
        Vector2 line = (Vector2) mousePosition - (Vector2) playerTransform.position;
        Vector2 direction = line.normalized;
        float theta = Mathf.Atan2(line.y, line.x);

        float length = (wX * wY) / Mathf.Sqrt(
                                            wX * wX * Mathf.Sin(theta) * Mathf.Sin(theta) 
                                            + wY * wY * Mathf.Cos(theta) * Mathf.Cos(theta)
                                            );


        return playerTransform.position + (Vector3) direction * length * line.magnitude;
    }

    private void OnDrawGizmos()
    {
        Vector3 mouse = new Vector3(mousePosition.x, mousePosition.y, 0);
        Gizmos.color = Color.white;
        if(mouse != null && playerTransform != null)
        {
        Gizmos.DrawLine(playerTransform.position, mouse);
        Gizmos.DrawWireSphere(cameraPosition, .8f);
        }
    }
}
