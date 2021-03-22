using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullHeart : MonoBehaviour
{
    public int total;
    [SerializeField] private RectTransform HeartJuice;
    [SerializeField] private RectTransform HeartJuiceShader;

    private int current;

    private void Awake()
    {
        current = total;
    }

    public void SetHealth( int health)
    {
        // idk how else to solve 
        if (current == health)
            return;
        if (current != health)
        {
            current = health;
            // Normalize to pixel values
            float norm = (5 * health - 80) / 3f;
            // Move juice and shader to current health
            HeartJuice.anchoredPosition = new Vector3(0, norm, 0);
            HeartJuiceShader.anchoredPosition = new Vector3(0, norm, 0);
        }
    }
}
