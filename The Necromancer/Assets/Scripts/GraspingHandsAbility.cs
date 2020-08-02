using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraspingHandsAbility : MonoBehaviour
{
    public float graspingHandsRadius = 1f;

    void Start()
    {
        transform.localScale = new Vector3(graspingHandsRadius, graspingHandsRadius, 0);
        Destroy(this.gameObject, PlayerController.graspingHandsEffectTime);
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
        EnemyController enenmyController = enemy.GetComponent<EnemyController>();
        // Freeze Enemy position 
        enenmyController.rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(enenmyController.holdTime);
        // If they are still alive unfreeze
        if (enemy != null) enenmyController.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
 