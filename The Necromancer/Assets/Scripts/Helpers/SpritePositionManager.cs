using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionManager : MonoBehaviour
{
    [SerializeField] private int sortOrderBase = 50000;
    [SerializeField] private int offset = 0;
    // If this object cannot move we run this script once after the update and destroy this component
    [SerializeField] private bool canMove = true;

    private Renderer myRenderer;
    private float timer;
    private float cooldown = .2f;

    private void Awake()
    {
        myRenderer = gameObject.GetComponent<Renderer>();
    }

    void LateUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = cooldown;
            // Every sprite will have thier order be propotional with thier y position
            myRenderer.sortingOrder = (int)(sortOrderBase - transform.position.y * 10 - offset);

            if (!canMove) Destroy(this);
        }
    }
}