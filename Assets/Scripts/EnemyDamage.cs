using UnityEngine;

public class EnemyDamage : MonoBehaviour
{

    private Animator anim;

    public int damage = 1;

    public Transform attackPoint;
    public float weaponRange;
    SkinnedMeshRenderer smr;
    public LayerMask playerLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        smr = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void Attacking()
    {

    }

    public void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, weaponRange, playerLayer);

        if (hits.Length > 0)
        {
            hits[0].GetComponent<PlayerHealth>().ChangeHealth(-damage);
        }
    }
}
