using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Drag your Bullet prefab here in the Inspector
    public Transform weaponMuzzle;   // Drag the weapon cube's Transform here in the Inspector
    public float fireRate = 0.2f;    // Time in seconds between shots
    private float nextFireTime = 0f;

    public float bulletSpawnOffset = 0.1f; 

    public GameObject muzzleFlashEffect; // Visual effect for the muzzleflash


    public AudioSource shootingSoundSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        // Handle shooting input (e.g., left mouse button)
        if (Time.timeScale > 0 && Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && weaponMuzzle != null)
        {
            // Calculate the spawn position slightly in front of the muzzle
            Vector3 spawnPosition = weaponMuzzle.position + weaponMuzzle.forward * bulletSpawnOffset;

            // Instantiate the bullet at the offset position and rotation
            GameObject bulletInstance = Instantiate(bulletPrefab, spawnPosition, weaponMuzzle.rotation);


            // Create shockwave
            if (muzzleFlashEffect != null)
            {
                

                GameObject muzzleFlashInstance = Instantiate(muzzleFlashEffect, spawnPosition, weaponMuzzle.rotation);
                // Destroy the muzzle flash effect after a specified duration
                Destroy(muzzleFlashInstance, .1f);
                
                // Get the Particle System component
                //ParticleSystem ps = muzzleFlashInstance.GetComponent<ParticleSystem>();
            }
            // Play the shooting sound
            if (shootingSoundSource != null)
            {
                shootingSoundSource.Play();
            }
            else
            {
                Debug.LogWarning("Shooting Audio Source not assigned!");
            }
        }
        else
        {
            Debug.LogError("Bullet Prefab or Weapon Muzzle not assigned in the Inspector!");
        }
    }
}
