using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Level3EnemyManager : MonoBehaviour
{

    [Header("Base Enemies")]
    public GameObject baseEnemyContainer; // Container holding your base enemy prefabs/instances
    private List<GameObject> activeBaseEnemies = new List<GameObject>();
    public bool baseEnemiesSpawned = false;
    public bool baseEnemiesDefeated = false;
    private List<GameObject> activeEnemies = new List<GameObject>();


    [Header("Boss Enemy")]
    public GameObject bossEnemy;
    private bool bossSpawned = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (baseEnemyContainer != null)
        {
            // Initially gather all base enemies and deactivate them
            foreach (Transform enemyTransform in baseEnemyContainer.transform)
            {
                GameObject enemy = enemyTransform.gameObject;
                if (enemy != null)
                {
                    enemy.SetActive(false);
                    activeEnemies.Add(enemy);
                }
            }
        }

        bossEnemy.SetActive(false); // Ensure the boss enemy is initially inactive

        
    }

    // Update is called once per frame
    void Update()
    {
        if (baseEnemiesSpawned){
            spawnBaseEnemy();

        }
        activeEnemies.RemoveAll(enemy => enemy == null); // Remove destroyed enemies
        if (activeEnemies.Count == 0)
        {
            baseEnemiesDefeated = true;
            if (!bossSpawned){
                spawnBoss();
                bossSpawned = true;
            }

        }
        

    }

    void spawnBaseEnemy(){
        foreach (Transform enemyTransform in baseEnemyContainer.transform)
        {
            GameObject enemy = enemyTransform.gameObject;
            if (enemy != null)
            {
                enemy.SetActive(true);
            }
        }
        baseEnemiesSpawned = false;

    }

    void spawnBoss(){
        bossEnemy.SetActive(true);
        bossSpawned = true;
    }
}
