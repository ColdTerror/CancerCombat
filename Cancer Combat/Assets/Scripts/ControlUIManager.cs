using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlUIManager : MonoBehaviour
{

    [Header("Scene Names")]
    public string MainMenuName = "MainMenu"; 

    
    public void backButton()
    {
        Debug.Log("Back to Menu");
        
        SceneManager.LoadScene(MainMenuName);
        Time.timeScale = 1f; // Resume the game time
    }

    
}
