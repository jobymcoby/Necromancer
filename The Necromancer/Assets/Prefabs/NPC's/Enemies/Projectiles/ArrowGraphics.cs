using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowGraphics : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator ani;

    void Start()
    {
        //Components
        rb = GetComponentInParent<Rigidbody2D>();
        ani = GetComponentInParent<Animator>();

        float rotation = 0;                         // Angle to turn the sprite to the right direction
        Vector2 projectileDirection = rb.velocity;  // Vector for the animato controller to get arrow

        // Graphics logic.
        // If arrow is facing the II quadrant
        if (projectileDirection.x < 0 && projectileDirection.y > 0)
        {
            rotation = 180;
            projectileDirection = projectileDirection * -Vector2.one;
        }
        // If arrow is facing the I quadrant
        else if (projectileDirection.y > 0)
        {
            // rotation minus 90
            rotation = 90;
            projectileDirection = projectileDirection * (Vector2.down + Vector2.right);
            projectileDirection = new Vector2(-projectileDirection.y, -projectileDirection.x);
        }
        // If arrow is facing the I quadrant
        else if (projectileDirection.x < 0)
        {
            // rotation plus 90
            rotation = -90;
            projectileDirection = projectileDirection * (Vector2.up + Vector2.left);
            projectileDirection = new Vector2(-projectileDirection.y, -projectileDirection.x);
        }
        // If arrow is facing the III quadrant do nothing because that is the orientation of the OG sprite sheet
        else { }

        //Set "minute" angle in animator
        ani.SetFloat("Vect X", projectileDirection.x);
        ani.SetFloat("Vect Y", projectileDirection.y);

        // Set quadrant angle in parent
        transform.parent.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
    }

}
