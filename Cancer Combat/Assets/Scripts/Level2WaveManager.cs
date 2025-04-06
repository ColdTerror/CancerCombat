using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq; 
using UnityEngine.SceneManagement;

public class Level2WaveManager : MonoBehaviour
{
    public GameObject[] waveContainers; 
    private int currentWaveIndex = 0;
    private bool canSpawnNextWave = false;
    private bool waveActive = false;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private bool allEnemiesDefeated = false; // Flag to check if all enemies are defeated
    public GameObject pipeToNextScene; //if player walks into this after all enemies defeated, load next scene
    public string nextSceneName;

    void Start()
    {
        canSpawnNextWave = true;

        // Ensure wave containers are assigned
        if (waveContainers == null || waveContainers.Length == 0)
        {
            Debug.LogError("Wave Containers not assigned in the WaveManager!");
            enabled = false;
            return;
        }

        // Deactivate all wave containers initially
        foreach (var container in waveContainers)
        {
            if (container != null)
            {
                container.SetActive(false);
            }
        }

        if (pipeToNextScene == null)
        {
             Debug.LogWarning("Pipe to Next Scene not assigned in WaveManager!");
        }


    }

    void Update()
    {
        if (waveActive)
        {
            // Check if all active enemies are defeated
            activeEnemies.RemoveAll(enemy => enemy == null); // Remove destroyed enemies
            if (activeEnemies.Count == 0)
            {
                waveActive = false;
                Debug.Log("Wave " + (currentWaveIndex + 1) + " completed!");

                // Move to the next wave if available
                currentWaveIndex++;
                if (currentWaveIndex < waveContainers.Length)
                {
                    canSpawnNextWave = true;
                    SpawnNextWave();
                }
                else
                {
                    Debug.Log("All waves completed for this level!");
                    allEnemiesDefeated = true;
                    StartCoroutine(LoadNextSceneWithDelay());

                }
            }
        }
        else if (canSpawnNextWave) 
        {
            SpawnNextWave();
        }
    }


    void SpawnNextWave()
    {
        if (currentWaveIndex < waveContainers.Length && canSpawnNextWave)
        {
            canSpawnNextWave = false;
            waveActive = true;

            GameObject currentWaveContainer = waveContainers[currentWaveIndex];
            if (currentWaveContainer != null)
            {
                currentWaveContainer.SetActive(true);
                Debug.Log("Spawning Wave " + (currentWaveIndex + 1));

                // Get all direct children (enemies) of the wave container
                foreach (Transform enemyTransform in currentWaveContainer.transform)
                {
                    GameObject enemy = enemyTransform.gameObject;
                    if (enemy != null && !activeEnemies.Contains(enemy))
                    {
                        activeEnemies.Add(enemy);
                    }
                }
            }
            else
            {
                Debug.LogError("Wave Container at index " + currentWaveIndex + " is null!");
            }
        }
    }

    public void LoadNextScene()
    {
        if (!allEnemiesDefeated)
        {
            Debug.LogWarning("Cannot load next scene. All enemies are not defeated yet.");
            return;
        }
        else{
            SceneManager.LoadScene(nextSceneName);
        }
        
    }

    IEnumerator LoadNextSceneWithDelay()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Loading next scene, onto the brain to finish the game");
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(nextSceneName);
    }

}