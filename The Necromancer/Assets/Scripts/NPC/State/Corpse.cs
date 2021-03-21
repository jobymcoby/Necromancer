using System;
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
    private GameObject body;
    private GameObject feet;

    public Corpse(GameObject gameObject, Rigidbody2D rb, Collider2D hitBox, GameObject body, GameObject feet)
    {
        this.gameObject = gameObject;
        this.rb = rb;
        this.hitBox = hitBox;
        this.body = body;
        this.feet = feet;
    }

    public void OnEnter()
    {
        // To specify which direction enemy will die, not implemented
        bool fallRight = false;
        DeathAnimation?.Invoke(fallRight); 

        // Stop movement, remove collisions
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

        if (gameObject.tag == "Enemy")
        {
            MakeFreshCorpse();
        }
        // Undead don't make corpse they fall apart and turn to dust.
        else
        {
            TurnToDust();
        }
    }

    private void MakeFreshCorpse()
    {
        // Set Tag for player rez
        gameObject.tag = "Corpse";
        body.tag = "Corpse";

        // hurtboxes no longer hit the gameobject
        hitBox.isTrigger = true;
        feet.GetComponent<DynamicTrigger>().enabled = false;
    }

    private void TurnToDust()
    {
        // Set Tag for player rez
        gameObject.tag = "Dead";
        body.tag = "Dead";

        // hurtboxes no longer hit the gameobject
        hitBox.isTrigger = true;
        feet.GetComponent<DynamicTrigger>().enabled = false;

        // Turn to dust and remove gameobject

    }

    public void Tick() { }
    public void FixedTick() { }
    public void OnExit() { }
}
