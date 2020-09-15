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
            StartCoroutine(enemy.gameObject.GetComponent<NPCController>().Grappled());
        }
    }
}
 