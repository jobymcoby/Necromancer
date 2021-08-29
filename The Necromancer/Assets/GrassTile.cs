using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : MonoBehaviour
{
    private GameObject Live;
    private GameObject Dead;
    [SerializeField] private Sprite[] LiveGrass;
    [SerializeField] private Sprite[] DeadGrass;

    private void Awake()
    {
        Live = transform.GetChild(0).gameObject;
        Dead = transform.GetChild(1).gameObject;
    }

    void Start()
    {
        // Access renderers
        SpriteRenderer rendLive = Live.GetComponent<SpriteRenderer>();
        SpriteRenderer rendDead = Dead.GetComponent<SpriteRenderer>();

        // Choose a random tile of the same type
        int randomTileOffset = UnityEngine.Random.Range(0, LiveGrass.Length);

        // find a random tile of the same type
        GetRandomTileStyle(rendLive, LiveGrass, randomTileOffset);
        GetRandomTileStyle(rendDead, DeadGrass, randomTileOffset);
    }

    private void GetRandomTileStyle(SpriteRenderer rend, Sprite[] Grass, int offset)
    {
        rend.sprite = Grass[offset];
    }
}
