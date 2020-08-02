using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public Vector3 mousePos;
    private Vector2 mousePos2D;
    public HealthSystem health;

    [SerializeField] private const float maxHealth = 30f;

    #region Movement (WASD)
    [SerializeField] private float moveSpeed = 5f; 
    public Rigidbody2D rb;
    private Vector2 movement;
    #endregion
    #region Grasping Hands (Right Click on Ground)
    public static float graspingHandsEffectTime = 3f;
    private float graspingHandsCoolDown = 4f;
    private float graspingHandsTimer = 0.0f;
    public GameObject graspingHandsArea;
    #endregion
    #region Resurrect (Right Click on Corpse)
    private Ray lClickRay;
    private RaycastHit2D hit;
    private bool hoverCorpse = false;       //Is the mouse hovering over a corpse
    private float resurrectCoolDown = 12f;
    private float resurrectTimer = 0.0f;
    #endregion
    #region Life Drain (Space Bar)
    private float lifeDrainCoolDown = .2f;
    private float lifeDrainAttackTimer = 0.0f;
    public GameObject lifeDrainCircle;
    #endregion
    #region Arcane Bolt (Left Click)

    #endregion

    void Start()
    {
       
        health = new HealthSystem(maxHealth);
        lifeDrainCircle.SetActive(false);
    }

    void Update()
    {
        #region Movement Polling
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        #endregion

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos2D = new Vector2(mousePos.x, mousePos.y);

        // Left Click Polling (Spell)
        if (Input.GetMouseButtonDown(0))
        {
            //Fire spell in mousePod2D.direction from the player
        }

        // Right Click Polling (Resurrect/Grasping Hands)
        if (Input.GetMouseButtonDown(1))                
        {
            // Send out ray to see what the mouse is over
            hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                // Resurrect
                if (hit.collider.gameObject.tag == "Corpse")    
                {
                    // Resurrect Cooldown Check
                    if (Time.time >= resurrectTimer)
                    {
                        // Restore Health, add to Horde
                        hit.collider.gameObject.GetComponent<EnemyController>().Resurrect();
                        resurrectTimer = Time.time + resurrectCoolDown;
                    }
                    else
                    {
                        // Resurrect in Cooldown 
                    }
                }
                // Grasping Hands
                else if (hit.collider.gameObject.tag == "Ground")   
                {
                    // Grasping Hands Cooldown Check
                    if (Time.time >= graspingHandsTimer)
                    {
                        // Create Grasping Hands 
                        Instantiate(graspingHandsArea, mousePos2D, Quaternion.identity);
                        graspingHandsTimer = Time.time + graspingHandsCoolDown;
                    }
                    else
                    {
                        // Grasping Hands in Cooldown
                    }
                }
                else
                {
                    // Clicked on not ground or corpse... idk man
                }
            }
            else
            {
                // Clicked on nothing
            }
        }

        // Space Button Polling (Life Drain)
        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time >= lifeDrainAttackTimer)
            { 
                lifeDrainCircle.SetActive(true);
            }
            else
            {
                // Pressed in cool down
            }
        }
        // Space Button Up (Life Drain Cooldown)
        if (Input.GetKeyUp(KeyCode.Space))
        {
            lifeDrainCircle.SetActive(false);
            lifeDrainAttackTimer = Time.time + lifeDrainCoolDown;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }
}
