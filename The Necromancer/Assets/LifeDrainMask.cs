using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeDrainMask : MonoBehaviour
{
    private SpriteMask mask;
    [SerializeField] private Sprite[] deathCircle;

    void Awake()
    {
        mask = GetComponent<SpriteMask>();
    }

    void Start()
    {
        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        int r = 0;
        while (r <= 29)
        {
            mask.sprite = deathCircle[r];
            r++;
            yield return new WaitForSeconds(.2f);
        }
    }
}
