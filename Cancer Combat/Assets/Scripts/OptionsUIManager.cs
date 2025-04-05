using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsUIManager : MonoBehaviour
{

    [Header("Scene Names")]
    public string MainMenuName = "MainMenu"; 
    
    bool isFullscreen = false; // Default value for fullscreen

    public void backButton()
    {
        Debug.Log("Back to Menu");
        
        SceneManager.LoadScene(MainMenuName);
        Time.timeScale = 1f; // Resume the game time
    }

    public void toggleFullscreen(bool input)
    {

        isFullscreen = !isFullscreen; // Toggle the fullscreen state

        Screen.fullScreen = isFullscreen;

        Debug.Log("Fullscreen toggled: " + isFullscreen);
    }
    
}
