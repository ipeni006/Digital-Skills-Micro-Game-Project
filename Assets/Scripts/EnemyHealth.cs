using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyHealth : MonoBehaviour
{
    [Header("Damage Effect Settings")]
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;
    private Color[] originalColors;
    private Renderer[] rends;

    [Header("Health Settings")]
    public int health;
    public int maxHealth = 10;

    public GameObject itemDrop;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<Renderer> validRends = new List<Renderer>();
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (r.material != null)
                validRends.Add(r);
        }
        rends = validRends.ToArray();

        health = maxHealth;

        originalColors = new Color[rends.Length];
        for (int i = 0; i < rends.Length; i++)
        {
            originalColors[i] = rends[i].material.color;
        }
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

        else if (health <= 0)
        {
            itemDrop.transform.position = transform.position;
            Destroy(gameObject);

        }
        if (amount < 0)
        {
            StartCoroutine(DoFlash());
        }
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
