using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_patrol : MonoBehaviour
{
    [Header("Ghost patroling points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Ghost")]
    [SerializeField] private Transform enemy;

    [Header("Moving parameter")]
    [SerializeField] private float speed;
    private Vector3 initscale;
    private bool movingLeft;

    [Header("Animation")]
    [SerializeField] private Animator anim;

    [Header("Idle Animation")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    private void Awake()
    {
        initscale = enemy.localScale;
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x > leftEdge.position.x)
            {
                MoveInDirection(-1);
            }
            else
            {
                DirectionChange(-1);
            }
        }
        else
        {
            if (enemy.position.x < rightEdge.position.x)
            {
                MoveInDirection(1);
            }
            else
            {
                DirectionChange(1);
            }
        }
    }

    private void DirectionChange(int direction)
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;
        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
            enemy.localScale = new Vector3(initscale.x * direction, initscale.y, initscale.z);
            idleTimer = 0;
        }
    }

    private void MoveInDirection(int direction)
    {
        idleTimer = 0;
        anim.SetBool("moving", true);
        enemy.localScale = new Vector3(initscale.x * direction, initscale.y, initscale.z);
        enemy.Translate(Vector3.right * Time.deltaTime * speed * direction);
    }
}