using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Alignment")]
public class AlignmentBehavoir : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // No neighbors stay straight
        if (context.Count == 0)
            return agent.transform.up;

        // Add the neighbors transforms together and average
        Vector2 alignMove = Vector2.zero;
        foreach (Transform item in context)
        {
            alignMove += (Vector2)item.transform.up;
        }
        // Center of the flock
        alignMove /= context.Count;

        return alignMove;
    }
}
