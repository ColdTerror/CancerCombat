using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;
    public float lifeSpan = 5f; // Destroy the bullet after this many seconds
    private Rigidbody rb;
    public int damageAmount = 5; // Set the bullet's damage in the Inspector

    public GameObject bulletCollideEffect;

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
        GameObject bulletCollideInstance = Instantiate(bulletCollideEffect, transform.position, Quaternion.identity);
        Destroy(bulletCollideInstance, .1f);
        // Check if the collided object has the "Enemy" tag
        if (collision.gameObject.CompareTag("Enemy"))
        {

            BaseEnemy baseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
            BossAI bossAI = collision.gameObject.GetComponent<BossAI>();

            if (baseEnemy != null)
            {
                // If it's a BaseEnemy, deal damage
                baseEnemy.TakeDamage(damageAmount);
            }
            else if (bossAI != null)
            {
                // If it's the BossAI, deal damage
                bossAI.TakeDamage(damageAmount);
            }
            else
            {
                Debug.LogWarning("Enemy hit but has no health script (BaseEnemy or BossAI): " + collision.gameObject.name);
                // Optionally destroy the enemy if no health script is found
                Destroy(collision.gameObject);
            }

        }
        Destroy(gameObject); // Destroy bullet 
    }

    public void setDamage(int damage)
    {
        damageAmount = damage;
    }
}