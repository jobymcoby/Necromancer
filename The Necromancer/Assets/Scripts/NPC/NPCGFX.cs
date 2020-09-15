using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGFX : MonoBehaviour
{
    // This will hold the subscription to events launched by the "NPC Controller" 
    // and translate that to the animator logic

    private Animator animator;
    private NPCController npc;
    private Rigidbody2D rb;

    private void Awake()
    {
        npc = GetComponentInParent<NPCController>();
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = npc.npcData.animator;
    }

    void Update()
    {
        //
        animator.SetFloat("LookX", -npc.facingDirection.x);
        animator.SetFloat("LookY", npc.facingDirection.y);
        animator.SetFloat("Velocity", rb.velocity.magnitude);
    }

    // attack, on hit, and on dead need to be add after the art.
}


