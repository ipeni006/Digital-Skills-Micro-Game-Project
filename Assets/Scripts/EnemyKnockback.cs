using System.Collections;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    private Rigidbody rb;
    private EnemyMovement enemyMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        enemyMovement = GetComponent<EnemyMovement>();
    }
    
    public void Knockback(Transform playerTransform, float knockbackForce, float stunTime)
    {
        enemyMovement.ChangeState(EnemyMovement.EnemyState.Knockback);
        StartCoroutine(StunTimer(stunTime));
        Vector3 direction = (transform.position - playerTransform.position).normalized;
        rb.linearVelocity = direction * knockbackForce;
        Debug.Log("Knockback applied to enemy");
    }

    IEnumerator StunTimer(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.angularVelocity = Vector2.zero;
        enemyMovement.ChangeState(EnemyMovement.EnemyState.Idle);
    }
}
