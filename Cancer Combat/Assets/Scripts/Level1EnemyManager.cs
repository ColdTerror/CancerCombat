using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class Level1EnemyManager : MonoBehaviour
{
    private bool allEnemiesDefeated = false;

    public GameObject enemyContainer; // Assign the empty children (enemies) in the Inspector
    private List<GameObject> activeEnemies = new List<GameObject>();

    public float delayBeforeNextScene = 5f; // Delay in seconds
    public string nextSceneName; // Name of the next scene

    public Level1CommanderBoltDialogue npcScript; // Reference to the NPC script
    
    void Start()
    {

        // Get all direct children (enemies) of the wave container
        foreach (Transform enemyTransform in enemyContainer.transform)
        {
            GameObject enemy = enemyTransform.gameObject;
            if (enemy != null && !activeEnemies.Contains(enemy))
            {
                activeEnemies.Add(enemy);
            }
        }
    }
    void Update()
    {
        if (!allEnemiesDefeated)
        {
        
            // Check if all active enemies are defeated
            activeEnemies.RemoveAll(enemy => enemy == null); // Remove destroyed enemies
            if (activeEnemies.Count == 0)
            {
                Debug.Log("All enemies defeated for this level!");
                npcScript.level1flag = true; // Set the flag to true in the NPC script
                allEnemiesDefeated = true;
                StartCoroutine(LoadNextSceneWithDelay());
                    
            }
        }
    
    }


    IEnumerator LoadNextSceneWithDelay()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSeconds(delayBeforeNextScene);
        SceneManager.LoadScene(nextSceneName);
    }
}
