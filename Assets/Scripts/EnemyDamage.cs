using UnityEngine;

public class EnemyDamage : MonoBehaviour
{

    private Animator anim;
    private EnemySoundManager soundManager;
    public int damage = 1;
    public float attackKnockback = 5;
    public float stunTime = 0.5f;

    public Transform attackPoint;
    public float weaponRange;
    SkinnedMeshRenderer smr;
    public LayerMask playerLayer;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
        soundManager = GetComponent<EnemySoundManager>();
    }

    // Update is called once per frame


    public void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, weaponRange, playerLayer);

        if (hits.Length > 0)
        {
            hits[0].GetComponent<PlayerHealth>().ChangeHealth(-damage);
            hits[0].GetComponent<PlayerController>().Knockback(transform, attackKnockback, stunTime);
            soundManager.PlayThwack();
        }
    }
}
