using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Components")]

    private Rigidbody2D rb;

    private SpriteRenderer sR;
    private Animator animator;

    private CharacterStats characterStats;

    [SerializeField] private bool flipNatural = false;

    [Header("Movement Settings")]

    [SerializeField] private float moveSpeed = 5f;
    private float moveMultiplier = 500f;

    public float moveRate = 1f;

    private Vector2 moveDirection;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sR = transform.GetChild(0).GetComponent<SpriteRenderer>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        characterStats = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    private void Update()
    {
        MovementAnimationController();
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveDirection * Time.deltaTime * moveSpeed * moveMultiplier * moveRate);
    }

    public void SetMoveDir(Vector2 dir)
    {
        moveDirection = dir;
    }

    private void MovementAnimationController()
    {
        if (!animator)
        {
            Debug.LogWarning("No animator");
            return;
        }
        if (!sR)
        {
            Debug.LogWarning("No Sprite Renderer");
            return;
        }

        if (characterStats.GetTarget() != null && characterStats.GetTarget() != characterStats)
        {
            Vector2 targetPos = characterStats.GetTarget().transform.position;

            if (moveDirection.x != 0 || moveDirection.y != 0)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            if (targetPos.y > transform.position.y)
            {
                animator.SetBool("Up", true);
            }
            else if (targetPos.y < transform.position.y)
            {
                animator.SetBool("Up", false);
            }

            if (targetPos.x > transform.position.x)
            {
                //sR.flipX = flipNatural;
                sR.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else if (targetPos.x < transform.position.x)
            {
                //sR.flipX = !flipNatural;
                sR.transform.localEulerAngles = new Vector3(0, 180, 0);
            }
        }
        else
        {
            if (moveDirection.x != 0 || moveDirection.y != 0)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            if (moveDirection.y > 0.1f)
            {
                animator.SetBool("Up", true);
            }
            else if (moveDirection.y < -0.1f)
            {
                animator.SetBool("Up", false);
            }

            if (moveDirection.x > 0.1f)
            {
                sR.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            else if (moveDirection.x < -0.1f)
            {
                sR.transform.localEulerAngles = new Vector3(0, 180, 0);
            }
        }
    }
}
