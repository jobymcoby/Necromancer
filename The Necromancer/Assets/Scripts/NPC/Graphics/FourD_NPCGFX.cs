using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourD_NPCGFX : NPCGraphicsBase
{
    void Update()
    {
        // Set Y
        animator.SetFloat("Look Y", npc.facingDirection.y);
        // Set X to the magnitude of they Y value, + values are rightfacing, - leftfacing
        ScaleCoordinateValue(npc.facingDirection.x);
        animator.SetFloat("Velocity", rb.velocity.magnitude);
    }

    private void ScaleCoordinateValue(float faceDirX)
    {
        if (Time.time >= flipTimer)
        {
            // if values arent on the same side of the zero, get them on the same side
            if (faceDirX < 0 && transform.localScale.x > 0)
            { 
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
                flipTimer = Time.time + flipCoolDown;
            }
            else if (faceDirX > 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
                flipTimer = Time.time + flipCoolDown;
            }
        }
    }
}