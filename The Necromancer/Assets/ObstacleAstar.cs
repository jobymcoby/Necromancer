using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class ObstacleAstar : MonoBehaviour
{
    void Awake()
    {
        // The 2d extras rule tile instatiates gameobject after A* pathfinding scans the level.
        // Rough fix is to rescan the A* grid after ruletiles are placed.
        StartCoroutine(UpdateObjectBounds());
    }

    private IEnumerator UpdateObjectBounds()
    {
        yield return new WaitForEndOfFrame();
        AstarPath.active.Scan();
        yield return null;
    }
}