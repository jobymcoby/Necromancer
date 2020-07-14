using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public Rigidbody2D rb;
    public Camera cam;
    private Vector2 movement;
    public Vector3 mousePosition;

    public static float graspingHandsEffectTime = 3f;
    private float graspingHandsCoolDown = 4f;
    private float graspingHandsAttackTimer = 0.0f;
    public GameObject graspingHandsArea;

    private float lifeDrainCoolDown = .5f;
    private float lifeDrainAttackTimer = 0.0f;
    public GameObject lifeDrainCircle;

    private bool hoverCorpse = false;       //Is the mouse hovering over a corpse

    private List<GameObject> enemiesDraining = new List<GameObject>();

    [SerializeField]
    private float maxHealth = 30f;
    [SerializeField]
    public HealthSystem health;


    void Start()
    {
        health = new HealthSystem(maxHealth);
        lifeDrainCircle.SetActive(false);
    }

    void Update()
    {
        // Player Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePosition = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 0);

        // Grasping Hands Input
        if (Input.GetMouseButtonDown(0) && hoverCorpse == false && Time.time >= graspingHandsAttackTimer)
        {
            Instantiate(graspingHandsArea, mousePosition, Quaternion.identity);
            graspingHandsAttackTimer = Time.time + graspingHandsCoolDown;
        }

        // Life Drain Input
        if (Input.GetKey(KeyCode.Space) && Time.time >= lifeDrainAttackTimer) lifeDrainCircle.SetActive(true);

        // Life Drain CoolDown
        if (Input.GetKeyUp(KeyCode.Space) && Time.time >= lifeDrainAttackTimer)
        {
            lifeDrainAttackTimer = Time.time + lifeDrainCoolDown;
            lifeDrainCircle.SetActive(false);
        }

        // idk about this attack
        if (Input.GetMouseButton(1))
        {
            //Fire Bolt
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }
}
