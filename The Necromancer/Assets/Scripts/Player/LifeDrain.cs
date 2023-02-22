using System;
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

    [SerializeField] private GameObject maskPrefab;

    void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        enemyMultiplier = 0;
        gameObject.SetActive(false);
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(KillPlants());
    }

    private IEnumerator KillPlants()
    {
        while (gameObject.activeSelf is true)
        {
            // Find the closest pixel cooridante to the mouse and put a mask there. 16 pixels per unit
            Vector2 pixelPoint = new Vector2(
                Mathf.Floor(transform.position.x * 16f),
                Mathf.Floor(transform.position.y * 16f)
            ) / 16f;

            GameObject circleMask = Instantiate(maskPrefab, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(.1f);
        }
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
        DealDamage?.Invoke(drainSpeed);                      
        player.Heal(drainSpeed * drainConversion * enemyMultiplier);   
    }

    private void OnDisable()
    {
        StopCoroutine(KillPlants());
        enemies.Clear();
    }
}
