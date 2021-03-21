using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraspingHandsAbility : DynamicTriggerListener
{
    public float graspingHandsRadius = 1f;
    private PlayerController player;
    private List<Rigidbody2D> grappledBodies = new List<Rigidbody2D>();


    void Start()
    {
        player = PlayerManager.instance.player.GetComponent<PlayerController>();
        transform.localScale = new Vector3(graspingHandsRadius, graspingHandsRadius, 0);
        Destroy(this.gameObject, PlayerController.graspingHandsEffectTime);
    }

    public override void OnDynamicTriggerEnter2D(Collider2D collision)
    {

        if (player.aggressionMatrix.CheckAggression(collision.gameObject.tag))
        {
            NPCData npc = collision.transform.root.gameObject.GetComponent<NPCController>().npcData;
            Rigidbody2D rb = collision.transform.root.gameObject.GetComponent<Rigidbody2D>();
            StartCoroutine(Grappled(rb, npc));
            grappledBodies.Add(rb);
        }
    }

    public IEnumerator Grappled(Rigidbody2D rb, NPCData npcData)
    {
        // Freeze Enemy position 
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(npcData.holdTime);
        // If they are still alive unfreeze
        if (this != null) rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void OnDynamicTriggerExit2D(Collider2D other)
    {

    }

    private void OnDestroy()
    {
        foreach (Rigidbody2D body in grappledBodies)
        {
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
 