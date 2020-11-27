using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixD_NPCGFX : NPCGraphicsBase
{

    void Update()
    {
        animator.SetFloat("LookX", npc.facingDirection.x);
        animator.SetFloat("LookY", npc.facingDirection.y);
        animator.SetFloat("Velocity", rb.velocity.magnitude);
    }
}


