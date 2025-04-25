using UnityEngine;
using UnityEngine.SceneManagement;


public class NextStoryStep : MonoBehaviour
{
    [Header("Scene Names")]
    public string Step1SceneName = "StoryStep1";
    public string Step2SceneName = "StoryStep1"; 
    public string Step3SceneName = "Level 1";
    public string Step4SceneName = "StoryStep4";
    public string Step5SceneName = "Level 2";
    public string Step6SceneName = "Level 3";
    public string Step7SceneName = "StoryStep7";     
    public string MainMenu = "MainMenu";  


    void Start()
    {
        Time.timeScale = 1f; // Ensure the game is running at normal speed
        Debug.Log(Time.timeScale);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Step1()
    {
        unlockCursor();
        Debug.Log("Loading Step 1");
        SceneManager.LoadScene(Step1SceneName);
    }
    public void Step2()
    {
        unlockCursor();
        Debug.Log("Loading Step 2");
        SceneManager.LoadScene(Step2SceneName);
    }
    public void Step3()
    {
        unlockCursor();
        Debug.Log("Loading Step 3");
        SceneManager.LoadScene(Step3SceneName);
    }
    public void Step4()
    {
        unlockCursor();
        Debug.Log("Loading Step 4");
        SceneManager.LoadScene(Step4SceneName);
    }
    public void Step5()
    {
        unlockCursor();
        Debug.Log("Loading Step 5");
        SceneManager.LoadScene(Step5SceneName);
    }
    public void Step6()
    {
        unlockCursor();
        Debug.Log("Loading Step 6");
        SceneManager.LoadScene(Step6SceneName);
    }
    public void Step7()
    {
        unlockCursor();
        Debug.Log("Loading Step 7");
        SceneManager.LoadScene(Step7SceneName);
    }

    public void EndGame(){
        unlockCursor();
        Debug.Log("Ending Game");
        SceneManager.LoadScene(MainMenu);
    }

    public void unlockCursor(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
}
