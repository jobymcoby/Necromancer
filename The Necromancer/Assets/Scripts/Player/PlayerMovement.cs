using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 mousePos;
    public Animator anim;

    #region Movement (WASD)
    [SerializeField] private float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Vector2 movement;
    #endregion

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        #region Movement Polling
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        #endregion

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        mousePos.Normalize();
        anim.SetFloat("MouseX", mousePos.x);
        anim.SetFloat("MouseY", mousePos.y);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.deltaTime);
    }
}
