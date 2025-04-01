using UnityEngine;
using TMPro;


public class PlayerManager : MonoBehaviour
{
    private bool isDead = false; // Flag to check if the player is dead
    public float maxHealth = 50; // Maximum health of the player
    private float currentHealth; // Current health of the player

    public GameObject loseTextObj; // Reference to the lose text object in the UI

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth; // Initialize the player's health to maximum at the start of the game
        loseTextObj.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetCurrentHealth()
    {
        // Returns the current health of the player
        return currentHealth;
    }

    public void TakeDamage(float damage)
    {
        // Reduces the player's current health by the damage amount
        currentHealth -= damage;

        Debug.Log("Player took damage: " + damage + ". Current health: " + currentHealth); // Log the damage taken and current health
        // Ensure current health doesn't go below 0
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
            
        }
    }

    void Die()
    {
        if (isDead) return; // Only die once

        Debug.Log("player has died"); // Log message when player dies
        loseTextObj.gameObject.SetActive(true);
        loseTextObj.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        isDead = true;

        // Pause the game by setting Time.timeScale to 0
        Time.timeScale = 0f;

    }
}
