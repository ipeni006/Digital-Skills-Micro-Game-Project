using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    private GameStateManager stateManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Damage Effect Settings")]
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    private Color[] originalColors;
    private Renderer[] rends;

    public Slider healthSlider;

    [Header("Health Settings")]
    public int maxHealth = 10;
    public int health;
    void Start()
    {
        stateManager = GetComponent<GameStateManager>();

        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;

        // Only keep renderers that have a valid material
        List<Renderer> validRends = new List<Renderer>();
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (r.material != null)
                validRends.Add(r);
        }
        rends = validRends.ToArray();

        

        originalColors = new Color[rends.Length];
        for (int i = 0; i < rends.Length; i++)
        {
            originalColors[i] = rends[i].material.color;
        }
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
            //stateManager.GameOver();
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        if (change < 0)
        {
            StartCoroutine(DoFlash());
        }
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;

    }
    private IEnumerator DoFlash()
    {
        int flashAmount = 1;
        while (flashAmount > 0)
        {
            // Set all renderers to flash color
            foreach (Renderer rend in rends)
                rend.material.color = flashColor;

            yield return new WaitForSeconds(flashDuration);

            // Restore each renderer's original color
            for (int i = 0; i < rends.Length; i++)
                rends[i].material.color = originalColors[i];

            yield return new WaitForSeconds(flashDuration);
            flashAmount--;
        }
    }
}
