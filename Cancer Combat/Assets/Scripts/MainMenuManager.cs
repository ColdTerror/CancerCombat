using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string startSceneName = "Game"; 
    public string optionsSceneName = "Options"; 
    public string creditsSceneName = "Credits"; 
    public string controlSceneName = "Controls";

    

    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Starting Game...");
        SceneManager.LoadScene(startSceneName);
    }

    public void OpenOptions()
    {
        Debug.Log("Opening Options Scene...");
        SceneManager.LoadScene(optionsSceneName);
    }

    public void OpenCredits()
    {
        Debug.Log("Opening Credits Scene...");
        SceneManager.LoadScene(creditsSceneName);
    }

    public void OpenControls()
    {
        Debug.Log("Opening Controls Scene...");
        SceneManager.LoadScene(controlSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

}