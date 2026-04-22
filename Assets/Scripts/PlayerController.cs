using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce; 
    public Vector2 move;

    private bool isJumping = false;
    public bool isInteracting = false;

    private bool isJumpPressed = false;

    private float maxJumpHeight = 2.0f;
    private float maxJumpTime = 0.5f;
    private float initialJumpVelocity;
    private CharacterController Controller;

    private Animator anim;

    Rigidbody rb;

    float gravity = -9.8f;
    float groundedGravity = -0.05f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        setupJumpVariables();

        anim = GetComponent<Animator>();

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
        Controller.Move(move * Time.deltaTime);
        anim.SetFloat("WalkSpeed", Mathf.Abs(move.x));
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        handleGravity();
        handleJump();

        if (move.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }

        if (move.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
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
            Debug.Log("JUMPINGG");
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

    public void OnMove(InputValue value)
    {
        move = new Vector3(value.Get<float>() * speed, transform.position.y, 0);

        Debug.Log(move.x);
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

}
