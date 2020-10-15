using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LifeDrainGFX : MonoBehaviour
{
    // Put this in it own class
    #region Drain Image
    public Color color;
    private SpriteRenderer[] image;
    #endregion
    #region Drain Constants
    [Range(0, 5)] public float drainRadius = 1.8f;
    #endregion

    private void Awake()
    {
        image = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer part in image)
        {
            part.color = new Color(color.r, color.g, color.b, part.color.a);
        }
        // Set radius of all objects
        transform.localScale = new Vector3(drainRadius / 1.8f, drainRadius / 1.8f, 1);
    }
}
