
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    public float speed = 10.0f;
    private GameObject player;
    public EnemyState enemyState;
    public float flipDir = 1;


    public float attackRange = 3;

    float idleTimer = 0;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        transform.rotation = Quaternion.Euler(0, 90, 0);
        ChangeState(EnemyState.Passive);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyState == EnemyState.Chasing)
            Chase();

        else if (enemyState == EnemyState.Attacking)
        {

            rb.linearVelocity = Vector3.zero; // ← stop moving when attacking
   
        }

        if (enemyState == EnemyState.Idle && idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0) 
            {
                ChangeState(EnemyState.Passive);
                rb.linearVelocity = Vector3.zero;
            }
        }
        

    }
    void Chase()
    {
            float direction = player.transform.position.x - transform.position.x;
            if(Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                ChangeState(EnemyState.Attacking);

            }

            else if (direction > 0)
            {
                rb.linearVelocity = new Vector3(speed, 0, 0);
                Flip(1);
            }

            else if (direction < 0)
            {
                rb.linearVelocity = new Vector3(-speed, 0, 0);
                Flip(-1);
            }
            anim.SetFloat("speed", speed);
    }

    private void Flip(int direction)
    {
        if (direction == 1)
            transform.rotation = Quaternion.Euler(0, 90 * flipDir, 0);
        else if (direction ==-1)
            transform.rotation = Quaternion.Euler(0, -90 * flipDir, 0);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (player == null)
                player = other.gameObject;
            
            ChangeState(EnemyState.Chasing);     
        }
            
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        { 
            ChangeState(EnemyState.Idle);
        }
    }

    void ChangeState(EnemyState newState)
    {
        // Exit current animation
        if (enemyState == EnemyState.Passive)
        {
            anim.SetBool("isPassive", false);
        }
        else if (enemyState == EnemyState.Idle)
        {

            anim.SetBool("isIdle", false);
        }
        else if (enemyState == EnemyState.Chasing)
        {
            // Set to false
            anim.SetBool("isMoving", false);
        }
        else if (enemyState == EnemyState.Attacking)
        {
            // Set to false
            anim.SetBool("isAttacking", false);
        }


        // Update state
        enemyState = newState;

        // Update the new animation
        if (enemyState == EnemyState.Passive)
        {
            anim.SetBool("isPassive", true);
        }
        else if (enemyState == EnemyState.Idle)
        {
            anim.SetBool("isIdle", true);
            anim.SetFloat("speed", 0);
            idleTimer = 10f;
        }
        else if (enemyState == EnemyState.Chasing)
        {
            // Set to true
            anim.SetBool("isMoving", true);
        }
        else if (enemyState == EnemyState.Attacking)
        {
            anim.SetBool("isAttacking", true);
        }


    }


    public void AttackEnd()
    {
        
        Debug.Log("Attack Ended");
        ChangeState(EnemyState.Idle);
    }
    public enum EnemyState
    {
        Passive,
        Idle,
        Chasing,
        Attacking,
    }
}
