using UnityEngine;

public class Level2ExitPipe : MonoBehaviour
{
    public Level2WaveManager waveManager; // Reference to the WaveManager



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && waveManager != null)
        {
            // Load the next scene when the player enters the pipe
            waveManager.LoadNextScene();
        }
    }
}
