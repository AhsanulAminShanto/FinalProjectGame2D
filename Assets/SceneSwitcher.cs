using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // This function will load the MenuScene
    public void GoToMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    // Optional: Function to quit the application
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!"); // This works in builds, but not in the editor
    }
}
