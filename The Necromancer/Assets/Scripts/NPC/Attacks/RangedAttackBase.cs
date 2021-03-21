using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackBase : MonoBehaviour, IAttack
{
    public GameObject projectilePrefab;
    public GameObject firePos;

    private NPCController npc;
    private float projectileMagnitude;
    private float attackRange;
    private float attackDamage;
    private ProjectileController projController;

    private void Start()
    {
        npc = GetComponentInParent<NPCController>();

        attackDamage = npc.npcData.attack1.attackDamage;
        projectileMagnitude = npc.npcData.attack1.projectileForce;
    } 

    // Called from an animation event when the character looks like they are firing, 
    // this is only called on the front facing animation and the blend tree makes it work for all anims.
    public void FireArrow()
    {
        // Vector from shooter to target
        Vector2 projectileDirection = npc.targeter.Distance.normalized;

        float rotation = 0;
        Vector3 cardinalDirection = projectileDirection * Vector3.one;
        Rigidbody2D rb;

        if (projectileDirection.x < 0 && projectileDirection.y > 0)
        {
            rotation = 180;
            cardinalDirection = projectileDirection * -Vector3.one;
        }
        else if (projectileDirection.y > 0)
        {
            // rotation minus 90
            rotation = 90;
            cardinalDirection = projectileDirection * (Vector3.down + Vector3.right);
            cardinalDirection = new Vector3(-cardinalDirection.y, -cardinalDirection.x, cardinalDirection.z);
        }
        else if (projectileDirection.x < 0)
        {
            // rotation plus 90
            rotation = -90;
            cardinalDirection = projectileDirection * (Vector3.up + Vector3.left);
            cardinalDirection = new Vector3(-cardinalDirection.y, -cardinalDirection.x, cardinalDirection.z);
        }
        else
        {
            cardinalDirection = projectileDirection * Vector3.one;
        }

        GameObject arrow = SetArrowDirectio(cardinalDirection, rotation);
        rb = arrow.GetComponentInChildren<Rigidbody2D>();
        projController = arrow.GetComponent<ProjectileController>();
        projController.damage = attackDamage;
        // Debug.Log("direction = " + projectileDirection + ". magniude = " + projectileMagnitude);

        rb.AddForce(projectileDirection * projectileMagnitude, ForceMode2D.Impulse);
    }

    private GameObject SetArrowDirectio(Vector2 normilzedDir, float rotation)
    {
        GameObject arrow = Instantiate(projectilePrefab, firePos.transform.position, Quaternion.Euler(new Vector3(0, 0, rotation)));
        Animator ani = arrow.GetComponentInChildren<Animator>();

        // Set Rotation by target destination
        ani.SetFloat("Vect X", normilzedDir.x);
        ani.SetFloat("Vect Y", normilzedDir.y);

        return arrow;
    }

}
