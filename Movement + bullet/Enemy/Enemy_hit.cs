using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_hit : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;

    [SerializeField] private int damage;

    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;


    private float cooldowntimer = Mathf.Infinity;
    private HP playerHealth;

    private void Update()
    {
        cooldowntimer += Time.deltaTime;
        if (playerInRange())
        {
            cooldowntimer = 0;
        }
    }
    private bool playerInRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right *range * transform.localScale.x, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
           , 0 ,Vector2.left,0, playerLayer );

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<HP>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
    private void DamagePlayer()
    {
        if (playerInRange())
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
