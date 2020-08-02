using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 mousePosition;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = GetComponentInParent<PlayerController>().mousePos - transform.position;
        mousePosition.Normalize();
        anim.SetFloat("MouseX", mousePosition.x);
        anim.SetFloat("MouseY", mousePosition.y);
    }
}
