using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DynamicTriggerListener : MonoBehaviour
{
    /// <summary>
    /// Called when the object begins intersecting a DynamicTrigger.
    /// </summary>
    abstract public void OnDynamicTriggerEnter2D(Collider2D other);

    /// <summary>
    /// Called when the object is no longer intersecting a DynamicTrigger,
    /// either because the collision volumes have moved, or because the
    /// DynamicTrigger was disabled or destroyed.
    /// </summary>
    abstract public void OnDynamicTriggerExit2D(Collider2D other);
}