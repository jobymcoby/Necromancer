using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    #region Singleton
    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject player;
    public Vector2 PlayerDirection(Rigidbody2D rb)
    {
        return ((Vector2)player.transform.position - rb.position).normalized;
    }
}
