using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IDamagable
{
    public Camera cam;
    public Ray clickRay;
    public HealthSystem health;

    [SerializeField] private const float maxHealth = 30f;

    public LayerMask clickLayers;
    #region Grasping Hands (Right Click on Ground)
    public static float graspingHandsEffectTime = 3f;
    private float graspingHandsCoolDown = 4f;
    private float graspingHandsTimer = 0.0f;
    public GameObject graspingHandsArea;
    #endregion
    #region Resurrect (Right Click on Corpse)
    private Ray lClickRay;
    private RaycastHit2D hit;
    private float resurrectCoolDown = 12f;
    private float resurrectTimer = 0.0f;

    public delegate void ResurrectHandler(GameObject ally);
    public event ResurrectHandler ResurrectEvent;
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
        ResurrectEvent += Resurrect;
    }

    private void Resurrect(GameObject ally)
    {
        // Play Rez Sounds
        // Add Enemy to horde
        // Rez anim and start cooldown
    }

    void Update()
    {
        clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Left Click Polling (Spell)
        if (Input.GetMouseButtonDown(0))
        {
            PrimarySpell();
        }

        // Right Click Polling (Resurrect/Grasping Hands)
        if (Input.GetMouseButtonDown(1))                
        {
            SecondarySpell();
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

    public void Damage(float dmg)
    {
        health.Damage(dmg);
    }

    private void PrimarySpell()
    {
        //Fire spell in mousePod2D.direction from the player
    }

    private void SecondarySpell()
    {
        // Send out ray to see what the mouse is over, I use the 3D vector here 
        // because negative in the Z axis is in front of the screen. 
        hit = Physics2D.Raycast(clickRay.origin, clickRay.direction);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.tag);
            // Resurrect
            if (hit.collider.gameObject.tag == "Corpse")
            {
                // Resurrect Cooldown Check
                if (Time.time >= resurrectTimer)
                {
                    // Restore Health, add to Horde
                    ResurrectEvent?.Invoke(hit.collider.gameObject);
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
                    Instantiate(graspingHandsArea, new Vector2(clickRay.origin.x, clickRay.origin.y), Quaternion.identity);
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

}
