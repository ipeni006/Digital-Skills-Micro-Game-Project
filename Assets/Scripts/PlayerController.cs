using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    [Header("Jump Settings")]
    public float maxJumpHeight = 1.0f;
    public float maxJumpTime = 0.5f;
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;      
    public bool isGrounded;          

    private Vector2 move;
    private float rawMoveInput;

    public bool isAttacking = false;
    private bool isJumping = false;
    public bool isInteracting = false;
    private bool isKnockedBack = false;

    private float initialJumpVelocity;
    private Vector3 knockbackVelocity = Vector3.zero;

    private CharacterController Controller;
    private PlayerCombat playerCombat;
    private Animator anim;



    float gravity = -9.8f;
    // Increased grounded gravity to firmly push the controller into the floor
    float groundedGravity = -2.0f;

    void Start()
    {
        Controller = GetComponent<CharacterController>();
        playerCombat = GetComponent<PlayerCombat>();
        anim = GetComponent<Animator>();

        setupJumpVariables();
        transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight / Mathf.Pow(timeToApex, 2));
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void Update()
    {
        // 1. Perform our custom, reliable ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isKnockedBack)
        {
            knockbackVelocity.y += gravity * Time.deltaTime;
            Controller.Move(knockbackVelocity * Time.deltaTime);
            knockbackVelocity.x = Mathf.MoveTowards(knockbackVelocity.x, 0, 15f * Time.deltaTime);

            if (isGrounded && knockbackVelocity.magnitude < 0.1f)
                isKnockedBack = false;

            return;
        }

        if (jumpBufferCounter > 0) jumpBufferCounter -= Time.deltaTime;

        handleGravity();
        handleJump();

        if (isAttacking)
        {
            move.x = 0;
            anim.SetFloat("WalkSpeed", 0);
        }
        else
        {
            move.x = rawMoveInput * speed;
            anim.SetFloat("WalkSpeed", Mathf.Abs(move.x));

            if (move.x < 0) transform.rotation = Quaternion.Euler(0, -90, 0);
            if (move.x > 0) transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        Controller.Move(new Vector3(move.x, move.y, 0) * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void handleGravity()
    {
        // Use our custom isGrounded instead
        if (isGrounded && move.y <= 0.0f)
        {
            move.y = groundedGravity;
            isJumping = false;
        }
        else
        {
            float oldYVelocity = move.y;
            float newYVelocity = move.y + (gravity * Time.deltaTime);
            move.y = (oldYVelocity + newYVelocity) * 0.5f;
        }
    }

    private void handleJump()
    {
        // Use our custom isGrounded instead
        if (isGrounded && jumpBufferCounter > 0f && !isJumping)
        {
            isJumping = true;
            move.y = initialJumpVelocity;
            jumpBufferCounter = 0f;
        }
    }

    public void OnJump(InputValue button)
    {
        if (button.isPressed)
        {
            jumpBufferCounter = jumpBufferTime;
        }
    }


    public void Knockback(Transform enemy, float knockbackAmount, float knockbackTime)
    {
        isKnockedBack = true;

        isKnockedBack = true;
        Vector3 direction = (transform.position - enemy.position).normalized;
        knockbackVelocity = direction * knockbackAmount;
        anim.SetFloat("WalkSpeed", Mathf.Abs(0));
        StartCoroutine(KnockbackCounter(knockbackTime));
    }
    
    IEnumerator KnockbackCounter(float knockbackTime)
    {
        yield return new WaitForSeconds(knockbackTime);
        knockbackVelocity = Vector3.zero;
        isKnockedBack = false;
    }
    public void OnMove(InputValue value)
    {
        rawMoveInput = value.Get<float>();
        if (isAttacking) return;

        move.x = rawMoveInput * speed;


    }
    public void OnInteract(InputValue button)
    {
        if (button.isPressed)
        {
            isInteracting = true;
        }
        else
        {
            isInteracting = false;
        }
    }

    public void OnAttack(InputValue button)
    {
        if (button.isPressed)
        {
            playerCombat.Attack();
        }
    }

}
