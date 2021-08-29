using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LifeDrain : DynamicTriggerListener
{
    #region Enemies Affected
    private Dictionary<GameObject, NPCHealth> enemies = new Dictionary<GameObject, NPCHealth>();
    private const float drainSpeed = .1f;
    private const float drainConversion = 0.60f;

    public delegate void DamageDealer(float dmg);
    public static event DamageDealer DealDamage;
    #endregion
    #region Player to Heal
    private PlayerController player;
    private int enemyMultiplier;
    #endregion

    void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        enemyMultiplier = 0;
    }

    // Triggers get the gameobjects to deal damage to
    public override void OnDynamicTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        try
        {
            bool aggroVal = player.aggressionMatrix.CheckAggression(collision.gameObject.tag);
        }
        catch (KeyNotFoundException e1)
        {
            throw;
        }
        finally
        {
            if (player.aggressionMatrix.CheckAggression(collision.gameObject.tag))
            {
                DealDamage += collision.gameObject.GetComponent<NPCHealth>().Damage;
                enemyMultiplier++;
            }
        }
    }

    public override void OnDynamicTriggerExit2D(Collider2D collision)
    {
        try
        {
            bool aggroVal = player.aggressionMatrix.CheckAggression(collision.gameObject.tag);
        }
        catch (KeyNotFoundException e1)
        {
            throw;
        }
        finally
        {
            if (player.aggressionMatrix.CheckAggression(collision.gameObject.tag))
            {
                DealDamage -= collision.gameObject.GetComponent<NPCHealth>().Damage;
                enemyMultiplier--;
            }
        }
    }

    // Damage is dealt every .2 seconds
    private void FixedUpdate()
    {
        Debug.Log(enemyMultiplier);
        DealDamage?.Invoke(drainSpeed);                      
        player.Heal(drainSpeed * drainConversion * enemyMultiplier);   
    }

    private void killGrass()
    {
        GrassTileManager.instance.StartDeathCircle(transform.position);
    }

    private void OnEnable()
    {
        InvokeRepeating("killGrass", 0, .2f);
    }

    private void OnDisable()
    {
        CancelInvoke();
        enemies.Clear();
    }
}
