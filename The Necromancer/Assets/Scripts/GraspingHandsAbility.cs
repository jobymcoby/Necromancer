using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraspingHandsAbility : MonoBehaviour
{
    public float graspingHandsRadius = 1f;

    void Start()
    {
        transform.localScale = new Vector3(graspingHandsRadius, graspingHandsRadius, 0);
        Destroy(this.gameObject, PlayerMovement.graspingHandsEffectTime);
    }

    private void OnTriggerEnter2D(Collider2D enemy)
    {
        if (enemy.gameObject.tag == "Enemy")
        {
            StartCoroutine("Grappled", enemy.gameObject);
        }
    }

    private IEnumerator Grappled(GameObject enemy)
    {
        // Freeze Enemy position 
        enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(enemy.gameObject.GetComponent<EnemyController>().holdTime);
        // If they are still alive unfreeze
        if (enemy != null) enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

    }
}
 