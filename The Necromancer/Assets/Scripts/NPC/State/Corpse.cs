using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : IState
{
    private GameObject gameObject;
    private Rigidbody2D rb;
    private Collider2D hitBox;
    private Animator animator;

    public Corpse(GameObject gameObject, Rigidbody2D rb, Collider2D hitBox)
    {
        this.gameObject = gameObject;
        this.rb = rb;
        this.hitBox = hitBox;

    }

    public void OnEnter()
    {
        // Set Tag
        gameObject.tag = "Corpse";
        Debug.Log("im dead");
        // Stop movement, remove collisions
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        hitBox.isTrigger = true;

    }

    public void Tick()
    {

    }

    public void FixedTick()
    {

    }

    public void OnExit()
    {
        // Play Raised anim
    }
}
