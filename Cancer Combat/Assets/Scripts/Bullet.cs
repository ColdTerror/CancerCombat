using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f;
    public float lifeSpan = 5f; // Destroy the bullet after this many seconds
    private Rigidbody rb;

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

    // Optional: Handle collisions
    void OnCollisionEnter(Collision collision)
    {
        // You can add logic here for what happens when the bullet hits something
        // For example, deal damage, play an effect, destroy the bullet
        Destroy(gameObject); // Destroy the bullet on impact for now
    }
}