using UnityEngine;
using UnityEngine.Rendering;

public class EnemyHealth : MonoBehaviour
{

    public int health;
    public int maxHealth = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeHealth(int amount)
    {
        health += amount;
        Debug.Log("Ouch");
        if (health > maxHealth) health = maxHealth;

        else if (health <=  0) Destroy(gameObject);

    }
}
