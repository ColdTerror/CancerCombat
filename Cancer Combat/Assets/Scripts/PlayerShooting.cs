using UnityEngine;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Drag your Bullet prefab here in the Inspector
    public Transform weaponMuzzle;   // Drag the weapon cube's Transform here in the Inspector
    public Transform cameraTransform;
    public float fireRate = 0.2f;    // Time in seconds between shots
    private float nextFireTime = 0f;

    public float bulletSpawnOffset = 0.1f; 

    public GameObject muzzleFlashEffect; // Visual effect for the muzzleflash


    public AudioSource shootingSoundSource;

    public int bulletDamage = 5;

    public GameObject damageUI;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damageUI.GetComponent<TextMeshProUGUI>().text = "Player Damage\n " + bulletDamage.ToString(); // Update the health UI text
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
            //Vector3 spawnPosition = weaponMuzzle.position + weaponMuzzle.forward * bulletSpawnOffset;
            Vector3 spawnPosition = weaponMuzzle.position + cameraTransform.forward * bulletSpawnOffset;

            // Instantiate the bullet at the offset position and rotation
            GameObject bulletInstance = Instantiate(bulletPrefab, spawnPosition, cameraTransform.rotation);

            
            // Get the Bullet component from the instance
            Bullet bulletComponent = bulletInstance.GetComponent<Bullet>();

            //Check if the bulletComponent exists before trying to set the damage.
            if (bulletComponent != null)
            {
                // Set the damage of the bullet
                bulletComponent.setDamage(bulletDamage);
            }
            else
            {
                Debug.LogError("Bullet prefab is missing the Bullet component!");
            }
            
            // Create shockwave
            if (muzzleFlashEffect != null)
            {
                

                GameObject muzzleFlashInstance = Instantiate(muzzleFlashEffect, spawnPosition, cameraTransform.rotation);
                // Destroy the muzzle flash effect after a specified duration
                Destroy(muzzleFlashInstance, .05f);
                
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
