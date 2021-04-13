using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGraphicsBase : MonoBehaviour
{
    public Animator animator;
    public GameObject rezPoint;
    protected NPCController npc;
    protected Rigidbody2D rb;
    protected float flipTimer = 0f;
    protected float flipCoolDown = 0.2f;
    private GameObject undeadPrefab;

    private void Awake()
    {
        npc = GetComponentInParent<NPCController>();
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Load info from NPC Data
        animator.runtimeAnimatorController = npc.npcData.animator;
    }

    public void AttackSwitch(bool val)
    {
        animator.SetBool("Attacking", val);
    }

    public void Die(bool dir)
    {
        // Set Direction if corpse cant fall in the other dir
        animator.SetTrigger("Death");
    }

    public void Resurrect(int undeadMode, GameObject undead)
    {
        animator.SetInteger("Undead Mode", undeadMode);
        undeadPrefab = undead;
    }

    public void CreateUndead()
    {
        Debug.Log(this.gameObject + " is going to be a " + undeadPrefab);
        Vector3 pos = rezPoint.transform.position;
        GameObject undead = Instantiate(undeadPrefab, pos, Quaternion.identity);
    }
}
