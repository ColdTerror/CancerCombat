using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For lists

public class BossAI : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    public float attackCooldown = 3f; // Time between attacks
    private float nextAttackTime;
    private int currentAttackPhase = 0; // To cycle through attacks

    // Attack 1: Spawn Enemies
    public GameObject baseEnemyPrefab;
    public Transform[] spawnPoints; // Array of positions to spawn enemies

    // Attack 2: Jump and Slam
    public float jumpForce = 10f;
    public float slamRadius = 5f;
    public float shockwaveForce = 5f;
    public LayerMask shockwaveTargetLayer; // Layers that the shockwave affects
    public GameObject shockwaveEffectPrefab; // Visual effect for the shockwave

    // Attack 3: Charge
    public float chargeSpeed = 15f;
    public float chargeDuration = 2f;
    private bool isCharging = false;
    private Vector3 chargeTargetPosition;

    // Boss Health 
    public int maxHealth = 30;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        nextAttackTime = Time.time + attackCooldown;
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

    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (Time.time >= nextAttackTime && player != null && !isCharging)
        {
            PerformAttack();
            nextAttackTime = Time.time + attackCooldown;
        }

        if (isCharging)
        {
            ChargeTowardsTarget();
        }
    }

    void PerformAttack()
    {
        currentAttackPhase++;
        currentAttackPhase %= 3; // Cycle through 0, 1, 2

        switch (currentAttackPhase)
        {
            case 0:
                SpawnEnemiesAttack();
                break;
            case 1:
                StartCoroutine(JumpSlamAttack());
                break;
            case 2:
                StartChargeAttack();
                break;
        }
    }

    // Attack 1: Spawn Enemies
    void SpawnEnemiesAttack()
    {
        Debug.Log("Boss is spawning enemies!");
        if (baseEnemyPrefab != null && spawnPoints.Length > 0)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                Instantiate(baseEnemyPrefab, spawnPoint.position, spawnPoint.rotation);
            }
        }
        else
        {
            Debug.LogWarning("Boss cannot spawn enemies: Prefab or spawn points not set.");
        }
    }

    // Attack 2: Jump and Slam
    IEnumerator JumpSlamAttack()
    {
        Debug.Log("Boss is jumping and slamming!");
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Initial jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            yield return new WaitForSeconds(0.5f); // Wait for the boss to go up

            // Slam down (you might need more sophisticated logic for landing)
            rb.linearVelocity = Vector3.down * (jumpForce * 2f); // Example force down
            while (!IsGrounded()) // Implement a way to check if the boss is grounded
            {
                yield return null;
            }
            rb.linearVelocity = Vector3.zero;

            // Create shockwave
            if (shockwaveEffectPrefab != null)
            {
                Instantiate(shockwaveEffectPrefab, transform.position, Quaternion.identity);
            }
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, slamRadius, shockwaveTargetLayer);
            foreach (Collider hitCollider in hitColliders)
            {
                // Apply force or damage to hit objects (e.g., the player)
                if (hitCollider.CompareTag("Player"))
                {
                    // Apply shockwave force away from the boss
                    Vector3 direction = (hitCollider.transform.position - transform.position).normalized;
                    if (hitCollider.GetComponent<Rigidbody>() != null)
                    {
                        hitCollider.GetComponent<Rigidbody>().AddForce(direction * shockwaveForce, ForceMode.Impulse);
                    }
                    // Or apply damage using a PlayerHealth script
                    // hitCollider.GetComponent<PlayerHealth>().TakeDamage(10);
                }
            }
        }
        else
        {
            Debug.LogError("Boss needs a Rigidbody for the Jump Slam attack.");
        }
    }

    bool IsGrounded()
    {
        // Implement a check to see if the boss is touching the ground
        // This could involve a raycast down from the boss's feet
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f);
    }

    // Attack 3: Charge
    void StartChargeAttack()
    {
        Debug.Log("Boss is charging!");
        isCharging = true;
        chargeTargetPosition = player.position; // Set the charge target
        // Optionally add a visual or sound cue for the charge
    }

    void ChargeTowardsTarget()
    {
        if (player != null)
        {
            // Get the current positions, ignoring the Y-axis
            Vector3 bossPositionXZ = new Vector3(transform.position.x, 0f, transform.position.z);
            Vector3 targetPositionXZ = new Vector3(chargeTargetPosition.x, 0f, chargeTargetPosition.z);

            float distanceXZ = Vector3.Distance(bossPositionXZ, targetPositionXZ);

            Vector3 direction = (chargeTargetPosition - transform.position).normalized; // Keep the original direction for movement
            Rigidbody rb = GetComponent<Rigidbody>();

            if (distanceXZ > 1.5f) // Check horizontal distance
            {
                rb.linearVelocity = direction * chargeSpeed;
            }
            else
            {
                
                rb.linearVelocity = Vector3.zero;
                isCharging = false;
                nextAttackTime = Time.time + attackCooldown + 1f; // Add a cooldown after charging
            }
        }
        else
        {
            isCharging = false;
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null) 
            {
                rb.linearVelocity = Vector3.zero;
            }
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    // Boss takes damage (call this from your bullet script)
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Boss took " + damageAmount + " damage. Current health: " + currentHealth);
        // Add visual/audio feedback for damage
    }

    void Die()
    {
        Debug.Log("Boss has been defeated!");
        // Add death effects, animations, etc.
        Destroy(gameObject);
    }
}