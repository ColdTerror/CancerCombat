using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifeSpan = 5f; // Destroy the bullet after this many seconds
    private Rigidbody rb;
    public int damageAmount = 5; // Set the bullet's damage in the Inspector


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Bullet needs a Rigidbody component!");
            Destroy(gameObject);
            return;
        }

        // Apply initial velocity
        rb.linearVelocity = transform.forward * speed;

        // Destroy the bullet after its lifespan
        Destroy(gameObject, lifeSpan);
    }


    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the "Enemy" tag
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Try to get the EnemyHealth component
            BaseEnemy BaseEnemy = collision.gameObject.GetComponent<BaseEnemy>();

            if (BaseEnemy != null)
            {
                // If the enemy has a script, deal damage
                BaseEnemy.TakeDamage(damageAmount);
            }
            else
            {
                Debug.LogWarning("Enemy hit but has no script: " + collision.gameObject.name);
                //destroy the enemy if no health script is found
                Destroy(collision.gameObject);
            }

        }
        Destroy(gameObject); // Destroy bullet 
    }
}