using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraspingHandSpawner : MonoBehaviour
{
    // 
    [SerializeField] private GameObject hand;
    private int numberHands = 11;

    // 
    private int radius = 16;
    private int edgeOffset = 4;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberHands; i++)
        {
            GameObject new_hand = Instantiate<GameObject>(hand, transform.position, Quaternion.identity, this.transform);
            new_hand.transform.localPosition = GetRandomPosition(spacing: 4);
        }
    }

    private Vector2 GetRandomPosition(int spacing)
    {
        float x = (float)Random.Range(-(radius - edgeOffset), (radius - edgeOffset)) / (float)radius;
        float y = (float)Random.Range(-(radius - edgeOffset), (radius - edgeOffset)) / (float)radius;
        //Check random coordinates 



        return new Vector2(x, y);
    }

}
