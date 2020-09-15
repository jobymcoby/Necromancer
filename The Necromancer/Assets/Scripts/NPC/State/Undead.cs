using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undead : IState
{
    private GameObject gameObject;
    private Rigidbody2D rb;
    private Collider2D hitBox;


    public Undead(GameObject gameObject, Rigidbody2D rb, Collider2D hitBox)
    {
        this.gameObject = gameObject;
        this.rb = rb;
        this.hitBox = hitBox;

    }


    public void OnEnter()
    {
        gameObject.tag = "Undead";
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        hitBox.isTrigger = false;

        Debug.Log("im undead ;)");
    }

    public void Tick()
    {

    }

    public void FixedTick()
    {

    }

    public void OnExit()
    {

    }
}
