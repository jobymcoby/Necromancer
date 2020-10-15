using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity does not call OnTriggerExit functions on objects when a trigger they're
/// colliding with becomes disabled or destroyed; this utility component implements
/// that missing behavior.
///
/// Objects that have a DynamicTrigger component should enable and disable this component,
/// and not the collider itself.
/// </summary>
public class DynamicTrigger : MonoBehaviour
{
    private Collider2D _collider;

    private bool _isTracking = true;
    private List<DynamicTriggerListener> _intersectingListeners = new List<DynamicTriggerListener>();

    private void Awake()
    {
        this._collider = GetComponent<Collider2D>();
        if (this._collider == null || !this._collider.isTrigger)
        {
            Debug.LogWarning($"{nameof(DynamicTrigger)}: Missing trigger collider!", this);
        }
    }

    /// <summary>
    /// Destroys the DynamicTrigger and its attached collider.
    /// </summary>
    public void DestroySelfAndCollider()
    {
        Destroy(this._collider);
        Destroy(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var listener = other.GetComponent<DynamicTriggerListener>();
        if (listener == null)
        {
            return;
        }

        this._intersectingListeners.Add(listener);
        if (this._isTracking)
        {
            listener.OnDynamicTriggerEnter2D(this._collider);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var listener = other.GetComponent<DynamicTriggerListener>();
        if (listener == null)
        {
            return;
        }

        var removed = this._intersectingListeners.Remove(listener);
        if (removed && this._isTracking)
        {
            listener.OnDynamicTriggerExit2D(this._collider);
        }
    }

    /// <summary>
    /// Notify intersecting objects that they've entered the trigger.
    /// </summary>
    private void ResumeTracking()
    {
        if (this._isTracking)
        {
            return;
        }

        this._isTracking = true;

        for (var ii = 0; ii < this._intersectingListeners.Count; ++ii)
        {
            var listener = this._intersectingListeners[ii];
            if (listener == null)
            {
                this._intersectingListeners.RemoveAt(ii);
                ii -= 1;
            }
            else
            {
                listener.OnDynamicTriggerEnter2D(this._collider);
            }
        }
    }

    /// <summary>
    /// Notify intersecting objects that they've exited this trigger
    /// </summary>
    private void StopTracking()
    {
        if (!this._isTracking)
        {
            return;
        }

        this._isTracking = false;

        for (var ii = 0; ii < this._intersectingListeners.Count; ++ii)
        {
            var listener = this._intersectingListeners[ii];
            if (listener == null)
            {
                this._intersectingListeners.RemoveAt(ii);
                ii -= 1;
            }
            else
            {
                listener.OnDynamicTriggerExit2D(this._collider);
            }
        }
    }

    private void OnEnable()
    {
        ResumeTracking();
    }

    private void OnDisable()
    {
        StopTracking();
    }

    private void OnDestroy()
    {
        StopTracking();
    }
}
