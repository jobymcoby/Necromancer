using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    public Rigidbody2D rb;
    public float moveSpeed = 3f;
    public float holdTime = 1f;

    private GameObject player;
    private Vector2 moveDir;

    public float health = 10f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        FindPlayerDirections();

        //die
        if (health < 0)
        {
            Destroy(this.gameObject);
        }

        //Grappled
    }



    private void FixedUpdate()
    {
        //Move player towards player
        rb.MovePosition(rb.position + moveDir * moveSpeed * Time.deltaTime);
    }

    private void FindPlayerDirections()
    {
        // Find the vector between the enemy and player and normalize it
        moveDir = player.GetComponent<Rigidbody2D>().position - rb.position;
        moveDir.Normalize();
    }

    public void Grappled()
    {
        //Accessed byt player controlle to start coroutines
        StartCoroutine("Grapple");
    }

    public IEnumerator Grapple()
    {
        float prevSpeed = moveSpeed;
        moveSpeed = 0;
        yield return new WaitForSeconds(holdTime);
        moveSpeed = prevSpeed;
    }

    public float LifeDrain(float speed)
    {
        return health -= speed * Time.deltaTime;
    }
}
