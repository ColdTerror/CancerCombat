using UnityEngine;
using TMPro;
using System.Collections.Generic;



public class PlayerManager : MonoBehaviour
{
    private bool isDead = false; // Flag to check if the player is dead
    public float maxHealth = 50; // Maximum health of the player
    private float currentHealth; // Current health of the player

    public GameObject textObj; // Reference to the lose text object in the UI
    public GameObject button;
    
    public DialogueBox DialogueBox;

    public bool inDialogue = false; // Flag to check if the player is in dialogue
    public PlayerMovement playerMovement;
    public PlayerShooting playerShooting;


    public GameObject gui;
    public GameObject healthUI;
    public GameObject damageUI;

    public void setMaxHealth(float health){
        maxHealth = health; // Set the maximum health of the player
        currentHealth = maxHealth; // Initialize current health to maximum
        setUIHealth(currentHealth); // Update the health UI text
        
    }
    public void setUIHealth(float health){
        healthUI.GetComponent<TextMeshProUGUI>().text = "Player Health\n " + currentHealth.ToString(); // Update the health UI text
    }

    public void setUIDamage(float damage){
        damageUI.GetComponent<TextMeshProUGUI>().text = "Player Damage\n " + damage.ToString(); // Update the health UI text

    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth; // Initialize the player's health to maximum at the start of the game
        textObj.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
        gui.gameObject.SetActive(true); 

        
        setUIHealth(currentHealth); // Set the initial health UI text
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueBox != null){
            if (DialogueBox.inDialogue || button.activeSelf)
            {
                Debug.Log("In dialogue"); // Log message when in dialogue
                
                // Disable player movement when in dialogue
                playerMovement.enabled = false;
                playerShooting.enabled = false; // Disable shooting when in dialogue
                Time.timeScale = 0f; 
            }
            else
            {
                // Enable player movement when not in dialogue
                playerMovement.enabled = true;
                playerShooting.enabled = true; // Enable shooting when not in dialogue
                Time.timeScale = 1f; 
            }
        }
        
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
        setUIHealth(currentHealth);
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

        gui.gameObject.SetActive(false);
        Debug.Log("player has died"); // Log message when player dies
        textObj.gameObject.SetActive(true);
        textObj.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        button.gameObject.SetActive(true);

        isDead = true;

        // Pause the game by setting Time.timeScale to 0
        Time.timeScale = 0f;

    }
}
