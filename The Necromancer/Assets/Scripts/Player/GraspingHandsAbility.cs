using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraspingHandsAbility : DynamicTriggerListener
{
    public float graspingHandsRadius = 1f;
    private PlayerController player;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        transform.localScale = new Vector3(graspingHandsRadius, graspingHandsRadius, 0);
        Destroy(this.gameObject, PlayerController.graspingHandsEffectTime);
    }

    public override void OnDynamicTriggerEnter2D(Collider2D collision)
    {
        if (player.aggressionMatrix.CheckAggression(collision.gameObject.tag))
        {
            StartCoroutine(collision.gameObject.GetComponent<NPCController>().Grappled());
        }
    }

    public override void OnDynamicTriggerExit2D(Collider2D other)
    {
        
    }
}
 