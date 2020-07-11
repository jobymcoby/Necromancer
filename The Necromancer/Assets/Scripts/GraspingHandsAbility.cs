using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraspingHandsAbility : MonoBehaviour
{
    private PlayerMovement player;
    private float radius;
    private float cooldownTime;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerMovement>();
        radius = player.graspingHandsRadius;
        this.transform.localScale = new Vector3(radius, radius, 0);
        cooldownTime = player.graspingHandsCooldown;
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        transform.position = player.mousePosition;
        StartCoroutine("RunEffect", cooldownTime);
    }

    private IEnumerator RunEffect( float effectTime)
    {
        // Apply hands to grappled enemy
        // set of more hand isn the circle with some distance bars to make it normal
        yield return new WaitForSeconds(effectTime);

        this.gameObject.SetActive(false); 
    }
}
 