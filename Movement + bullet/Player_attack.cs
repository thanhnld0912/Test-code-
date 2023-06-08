using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_attack : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;
    private Move playerMovement;
    private float cooldown = Mathf.Infinity;

    [SerializeField] private float AttackCooldown;
    [SerializeField] private Transform fire;
    [SerializeField] private GameObject[] bullets;
    // Update is called once per frame
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<Move>();
    }
    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldown > AttackCooldown && playerMovement.canAttack())
        {
            Attack();
        }
        cooldown += Time.deltaTime;
    }
    private void Attack()
    {
        anim.SetTrigger("attack");
        cooldown = 0;

        bullets[FindBullets()].transform.position = fire.position;
        bullets[FindBullets()].GetComponent<Bullet>().SetDiretion(Mathf.Sign(transform.localScale.x));

    }
    private int FindBullets()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}
