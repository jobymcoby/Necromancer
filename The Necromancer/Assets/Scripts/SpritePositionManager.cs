using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionManager : MonoBehaviour
{
    // Start int
    [SerializeField] private int sortOrderBase = 5000;
    private Renderer myRenderer;

    private void Awake()
    {
        myRenderer = gameObject.GetComponent<Renderer>();
    }

    void LateUpdate()
    {
        // Every sprite will have thier order be propotional with thier y position
        myRenderer.sortingOrder = (int)(sortOrderBase - transform.position.y);
    }
}
