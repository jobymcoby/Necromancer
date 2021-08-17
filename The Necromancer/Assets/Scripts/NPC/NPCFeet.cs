using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NPCFeet : MonoBehaviour
{
    Collider2D col;
    DynamicGridObstacle dgo;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        dgo = GetComponent<DynamicGridObstacle>();
    }

    private void OnDestroy()
    {
        col.enabled = false;
        dgo.DoUpdateGraphs();
        AstarPath.active.FlushGraphUpdates();
    }
}
