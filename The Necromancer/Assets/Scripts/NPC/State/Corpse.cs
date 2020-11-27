using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : IState
{
    public delegate void DeathHappens(bool dir);
    public event DeathHappens DeathAnimation;

    private GameObject gameObject;
    private Rigidbody2D rb;
    private Collider2D hitBox;

    public Corpse(GameObject gameObject, Rigidbody2D rb, Collider2D hitBox)
    {
        this.gameObject = gameObject;
        this.rb = rb;
        this.hitBox = hitBox;
    }

    public void OnEnter()
    {
        bool fallRight = false;
        DeathAnimation?.Invoke(fallRight); 
        // Set Tag
        gameObject.tag = "Corpse";

        // Stop movement, remove collisions
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        hitBox.isTrigger = true;
    }

    public void Tick() { }
    public void FixedTick() { }
    public void OnExit() { }
}
