using UnityEngine;
using UnityEngine.SceneManagement;

public class EndUIManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string MainMenuName = "MainMenu"; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GoToMenu()
    {
        Debug.Log("Back to Menu");
        SceneManager.LoadScene(MainMenuName);
    }
}
