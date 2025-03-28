using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMeshAgent;
    public int maxHealth = 10;
    private int currentHealth;

    public event System.Action OnDeath; // Optional: Event to notify other scripts when the enemy dies

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Could not find player with tag 'Player'!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);
        }

        // Optional: Add visual feedback for low health
        if (currentHealth <= 0)
        {
            // Ensure Die() is only called once
            if (navMeshAgent.enabled)
            {
                Die();
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Current health: " + currentHealth);

    }

    void Die()
    {
        navMeshAgent.enabled = false;
        Debug.Log(gameObject.name + " has died!");
        if (OnDeath != null)
        {
            OnDeath.Invoke(); // Notify any listeners that this enemy has died
        }
        Destroy(gameObject);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}