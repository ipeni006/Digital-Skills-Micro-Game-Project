using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class PlayerCombat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator anim;
    private PlayerController playerController;

    public Transform attackPoint;
    public float weaponRange = 1;
    public int damage = 1;
    public float knockbackForce = 50;
    public float stunTime = 1f;
    public LayerMask enemyLayer;
    public float cooldown = 1f;
    private float timer;


    

    void Start()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
    public void Attack()
    {
        if (timer <= 0)
        {
            playerController.isAttacking = true;
            anim.SetBool("isAttacking", true);

 
        }
    }

    public void DoAttack()
    {
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position + transform.forward *0.5f, weaponRange, enemyLayer);

        Debug.Log(enemies.Length);
        if (enemies.Length > 0)
        {

            enemies[0].GetComponent<EnemyHealth>().ChangeHealth(-damage);
            enemies[0].GetComponent<EnemyKnockback>().Knockback(transform, knockbackForce, stunTime);
        }

        Debug.DrawRay(attackPoint.position + transform.forward * 0.5f, Vector3.up, Color.red, 1f);
        timer = cooldown;
    }

    public void EndAttack()
    {
        playerController.isAttacking = false;
        anim.SetBool("isAttacking", false);

    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position + transform.forward * 0.5f, weaponRange);
    }

}
