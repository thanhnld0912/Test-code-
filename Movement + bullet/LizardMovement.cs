using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardMovement: MonoBehaviour
{
    [Header("Ghost patroling points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Ghost")]
    [SerializeField] private Transform enemy;

    [Header("Moving parameter")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpInterval; // new parameter for jump interval
    [SerializeField] private float jumpDelay; // new parameter for jump delay
    [SerializeField] private float moveDelay; // new parameter for move delay

    private bool isMoving; // new variable for move state

    private bool movingLeft;
    private Vector3 initscale;

    [Header("Animation")]
    [SerializeField] private Animator anim;

    private Rigidbody2D rb; // new variable for Rigidbody2D

    private float jumpTimer; // new variable for jump timer

    private bool isJumping; // new variable for jump state


    private void Awake()
    {
        initscale = enemy.localScale;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isJumping && !isMoving) // check if not jumping and not moving
        {
            if (movingLeft)
            {
                if (enemy.position.x > leftEdge.position.x)
                {
                    MoveInDirection(-1);
                }
                else
                {
                    DirectionChange(1); // change direction to move right instead of flipping scale
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
                    DirectionChange(-1); // change direction to move left instead of flipping scale
                }
            }
        }
        else if (isJumping && rb.velocity.y < 0.01f) // check if jumping and landed
        {
            StartCoroutine(MoveDelay());
            isJumping = false;
        }

        jumpTimer += Time.deltaTime; // increment jump timer

        if (!isJumping && jumpTimer >= jumpInterval) // check if enough time has passed
        {
            AutoJump();
            jumpTimer = 0; // reset jump timer
        }
    }
    private void DirectionChange(int direction)
    {
        isMoving = true; // set move state to true
        movingLeft = !movingLeft;
        enemy.localScale = new Vector3(initscale.x * direction, initscale.y, initscale.z);
    }

    private void MoveInDirection(int direction)
    {
        isMoving = true; // set move state to true
        enemy.localScale = new Vector3(initscale.x * direction, initscale.y, initscale.z);
        enemy.Translate(Vector3.right * Time.deltaTime * speed * direction);
    }

    private IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(jumpDelay);
    }

    private IEnumerator MoveDelay()
    {
        isMoving = true; // set move state to true
        yield return new WaitForSeconds(moveDelay);
        isMoving = false; // set move state to false
        DirectionChange(movingLeft ? -1 : 1);
    }

    private void AutoJump()
    {
        if (rb != null && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(JumpDelay());
            isJumping = true;
        }
    }
}