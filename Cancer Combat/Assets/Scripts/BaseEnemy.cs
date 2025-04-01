using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMeshAgent;
    public int maxHealth = 10;
    public int damage = 5;
    public int pushForce = 5;
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerManager component from the player
            PlayerManager playerManager = collision.gameObject.GetComponent<PlayerManager>();
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            if (playerManager != null)
            {
                // Deal damage to the player
                playerManager.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("Player hit but has no PlayerManager script!");
            }

            if (playerRb != null)
            {
                // Calculate the direction to knock the player back
                Vector3 knockbackDirection = (collision.transform.position - transform.position).normalized;
                // Apply an upward component to make them fly back and up a bit
                knockbackDirection.y = 0.5f; // Adjust this value for the vertical lift
                playerRb.AddForce(knockbackDirection * pushForce, ForceMode.Impulse);
            }
            else
            {
                Debug.LogWarning("Player hit by charge but has no Rigidbody for knockback!");
            }

            //destroy base enemy upon contact with player
            Die();
        }
    }
}