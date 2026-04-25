using JetBrains.Annotations;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce; 
    private Vector2 move;
    private float rawMoveInput;

    public bool isAttacking = false;
    private bool isJumping = false;
    public bool isInteracting = false;

    private bool isJumpPressed = false;

    private float maxJumpHeight = 2.0f;
    private float maxJumpTime = 0.5f;
    private float initialJumpVelocity;
    private CharacterController Controller;
    private PlayerCombat playerCombat;

    private Vector3 knockbackVelocity = Vector3.zero;

    private Animator anim;
    Rigidbody rb;

    private bool isKnockedBack = false;

    float gravity = -9.8f;
    float groundedGravity = -0.05f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
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
    // Update is called once per frame
    void Update()
    {
        if (isKnockedBack == true)
        {
            knockbackVelocity.y += gravity * Time.deltaTime;
            Controller.Move(knockbackVelocity * Time.deltaTime);

            knockbackVelocity.x = Mathf.MoveTowards(knockbackVelocity.x, 0, 15f * Time.deltaTime);

            if (Controller.isGrounded && knockbackVelocity.magnitude < 0.1f)
                isKnockedBack = false;

            return;
        }

        handleGravity();
        handleJump();


        if (isAttacking)
        {
            move.x = 0; // Zero out horizontal input while attacking
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
        if (Controller.isGrounded)
        {
            move.y = groundedGravity;
        }
        else
        {
            // To average falling down velocity while accounting for framerate
            // Velocity verlet
            float oldYVelocity = move.y;
            float newYVelocity = move.y + (gravity * Time.deltaTime);
            float nextYVelocity = (oldYVelocity + newYVelocity) * .5f;
            move.y = nextYVelocity;
        }
    }

    private void handleJump()
    {
        if (!isJumping && Controller.isGrounded && isJumpPressed)
        {
   
            isJumping = true;
            move.y = initialJumpVelocity * .5f;
        }
        else if(isJumpPressed && isJumping && Controller.isGrounded)
        {
            isJumping = false;
        }
    }
    
    public void OnJump(InputValue button)
    {
        if (button.isPressed)
        {
            Debug.Log("Jump started (Button Held)");
            isJumpPressed = true;
        }
        else
        {
            Debug.Log("Jump stopped (Button Released)");
            isJumpPressed = false;
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
