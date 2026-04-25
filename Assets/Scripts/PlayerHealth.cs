using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private GameStateManager stateManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int maxHealth = 10;
    public int health;
    void Start()
    {
        stateManager = GetComponent<GameStateManager>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeHealth(int change)
    { 
         
        health += change;
        if (health > maxHealth) health = maxHealth;
        if (health <= 0)
        {
            stateManager.GameOver();
        }
          
    }
}
