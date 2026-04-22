using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        if(collision.gameObject.tag == "Player")
            collision.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
    }
}
