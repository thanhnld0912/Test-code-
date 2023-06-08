using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private float speed = 8f;
    private Rigidbody2D body;
    private Animator anim;
    private float horizontalInput;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    private void Awake()
    {
        //Grab references for Rigidbody and Animator
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        MovePlayer();
        FlipPlayer();
        CheckGrounded();
        CheckJump();
        SetAnimatorParameters();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        Debug.Log("Horizontal Input: " + horizontalInput);
    }

    private void MovePlayer()
    {
        Vector2 movement = new Vector2(horizontalInput * speed, body.velocity.y);
        body.velocity = movement;
    }

    private void FlipPlayer()
    {
        if (horizontalInput > 0.01f)
        {
            transform.GetChild(0).localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
        }
    }

    private void CheckGrounded()
    {
        anim.SetBool("grounded", isGrounded());
    }

    private void CheckJump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded())
        {
            Jump();
        }
    }

    private void SetAnimatorParameters()
    {
        anim.SetBool("run", horizontalInput != 0);
        anim.SetTrigger("jump");
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        anim.SetTrigger("jump");
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded();
    }
}