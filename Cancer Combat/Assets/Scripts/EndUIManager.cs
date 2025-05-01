using UnityEngine;
using UnityEngine.SceneManagement;

public class EndUIManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string MainMenuName = "MainMenu"; 

    public GameObject menuButton;

    void Update(){
        
        if (menuButton.activeInHierarchy){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
        }
    }
    public void GoToMenu()
    {
        Debug.Log("Back to Menu");
        
        SceneManager.LoadScene(MainMenuName);
        Time.timeScale = 1f; // Resume the game time
    }
}
