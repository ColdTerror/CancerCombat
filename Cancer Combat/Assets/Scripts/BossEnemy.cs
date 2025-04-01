using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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
    public float shockwaveForce = 10f;
    public float slamDamage = 10; // Damage dealt to the player on slam
    public LayerMask shockwaveTargetLayer; // Layers that the shockwave affects
    public GameObject shockwaveEffectPrefab; // Visual effect for the shockwave

    // Attack 3: Charge
    public float chargeSpeed = 15f;
    public float chargeDuration = 2f;
    public float chargeForce = 5f; // Force applied to the player upon collision
    public float chargeDamage = 10; // Damage dealt to the player on charge
    private bool isCharging = false;
    private Vector3 chargeTargetPosition;

    // Boss Health 
    public int maxHealth = 100;
    private int currentHealth;


    //UI

    public GameObject textObj;
    public GameObject winButton;

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

        textObj.gameObject.SetActive(false);
        winButton.gameObject.SetActive(false);
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
        Collider bossCollider = GetComponent<Collider>(); // Get the boss's collider

        if (rb != null && bossCollider != null)
        {
            // Initial jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            yield return new WaitForSeconds(0.5f); // Wait for the boss to go up

            rb.linearVelocity = Vector3.down * (jumpForce * 2f); //slam downwards by setting a downward velocity
            while (!IsGrounded()) // wait till the boss is grounded
            {
                yield return null;
            }
            rb.linearVelocity = Vector3.zero;


            // Create shockwave
            if (shockwaveEffectPrefab != null)
            {
                Debug.Log("Shockwave effect instantiated!");
                Vector3 spawnPosition = bossCollider.bounds.min; // Get the bottom-most point of the collider
                GameObject shockwaveInstance = Instantiate(shockwaveEffectPrefab, spawnPosition, Quaternion.identity);
                shockwaveInstance.transform.Rotate(-90f, 0f, 0f);

                // Get the Particle System component
                ParticleSystem ps = shockwaveInstance.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    // Access the Shape module
                    var shape = ps.shape;
                    // Set the radius to match the slamRadius
                    shape.radius = slamRadius;
                }
                else
                {
                    Debug.LogWarning("Shockwave effect prefab does not have a Particle System component.");
                }
            }
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, slamRadius, shockwaveTargetLayer);
            foreach (Collider hitCollider in hitColliders)
            {
                Debug.Log("Hit object: " + hitCollider.name); // For debugging purposes
                // Apply force or damage to hit objects (e.g., the player)
                if (hitCollider.CompareTag("Player"))
                {
                    // Apply shockwave force away from the boss
                    Vector3 direction = (hitCollider.transform.position - transform.position).normalized;
                    if (hitCollider.GetComponent<Rigidbody>() != null)
                    {
                        hitCollider.GetComponent<Rigidbody>().AddForce(direction * shockwaveForce, ForceMode.Impulse);
                    }
                    hitCollider.GetComponent<PlayerManager>().TakeDamage(slamDamage);
                }
            }
        }
        else
        {
            Debug.LogError("Boss needs a Rigidbody/Collider for the Jump Slam attack.");
        }
    }

    bool IsGrounded()
    {

        Collider bossCollider = GetComponent<Collider>();
        if (bossCollider == null) return false; // No collider, can't be grounded

        float raycastDistance = 0.3f; // Adjust this as needed
        Vector3 raycastOrigin = bossCollider.bounds.min + Vector3.up * 0.1f; // Start slightly above the bottom


        return Physics.Raycast(raycastOrigin, Vector3.down, raycastDistance);
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
            else //end of charge
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


    void OnCollisionEnter(Collision collision)
    {
        if (isCharging && collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerManager component from the player
            PlayerManager playerManager = collision.gameObject.GetComponent<PlayerManager>();
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();

            if (playerManager != null)
            {
                // Deal damage to the player
                playerManager.TakeDamage(chargeDamage);
            }
            else
            {
                Debug.LogWarning("Player hit by charge but has no PlayerManager script!");
            }

            if (playerRb != null)
            {
                // Calculate the direction to knock the player back
                Vector3 knockbackDirection = (collision.transform.position - transform.position).normalized;
                // Apply an upward component to make them fly back and up a bit
                knockbackDirection.y = 0.5f; // Adjust this value for the vertical lift
                playerRb.AddForce(knockbackDirection * chargeForce, ForceMode.Impulse);
            }
            else
            {
                Debug.LogWarning("Player hit by charge but has no Rigidbody for knockback!");
            }

            // Stop the charge after hitting the player
            isCharging = false;
            Rigidbody bossRb = GetComponent<Rigidbody>();
            if (bossRb != null) bossRb.linearVelocity = Vector3.zero;
            nextAttackTime = Time.time + attackCooldown;
        }
        // Check for obstacle collision during charge (using Tag)
        else if (isCharging && collision.gameObject.CompareTag("Obstacle"))
        {
            StopChargeOnObstacleHit();
        }
    }

    void StopChargeOnObstacleHit()
    {
        Debug.Log("Boss hit an obstacle while charging!");
        isCharging = false;
        Rigidbody bossRb = GetComponent<Rigidbody>();
        if (bossRb != null) bossRb.linearVelocity = Vector3.zero;
        nextAttackTime = Time.time + attackCooldown;
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
        Destroy(gameObject);

        // Pause the game by setting Time.timeScale to 0
        Time.timeScale = 0f;

        textObj.gameObject.SetActive(true);
        textObj.GetComponent<TextMeshProUGUI>().text = "You Win!";

        winButton.gameObject.SetActive(true);

    }
}