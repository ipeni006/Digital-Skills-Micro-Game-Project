
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class EnemyMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    public float playerDetectRange = 5;
    public Transform detectionPoint;
    public LayerMask playerLayer;

    public float attackCooldown = 2f;
    public float speed = 10.0f;
    public float flipDir = 1;
    public float attackRange = 3;

    public float attackCooldownTimer;
    
    private Transform player;
    public EnemyState enemyState;

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
        if(enemyState != EnemyState.Knockback)
        {
            CheckForPlayer();
            if (attackCooldownTimer > 0)

                attackCooldownTimer -= Time.deltaTime;

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

                }
            }
        }
    }
    void Chase()
    {
            float direction = player.position.x - transform.position.x;

            if (direction > 0)
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

    private void CheckForPlayer()
    {

        Collider[] hits = Physics.OverlapSphere(detectionPoint.position, playerDetectRange, playerLayer);

        if(hits.Length > 0)
        {
            player = hits[0].transform;
            if (hits[0].GetComponent<PlayerController>().isInteracting == false && enemyState == EnemyState.Passive) return; 

            else if (Vector3.Distance(transform.position, player.position) <= attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);

            }
            else if(Vector3.Distance(transform.position, player.position) > attackRange && enemyState != EnemyState.Attacking)
            {
                ChangeState(EnemyState.Chasing);
            }

            
        }
        else
        {
            rb.linearVelocity = Vector3.zero;

        }

    }


    public void ChangeState(EnemyState newState)
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
        else if (enemyState == EnemyState.Knockback)
        {
            anim.SetBool("isKnockedback", false);
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
        else if (enemyState == EnemyState.Knockback)
        {
            anim.SetBool("isKnockedback", true);
        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(detectionPoint.position, playerDetectRange);
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
        Knockback
    }
}
