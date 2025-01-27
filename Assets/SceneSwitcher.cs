using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioSource musicSource;

    // This function will load the MenuScene
    public void GoToMenuScene()
    {
        // Load the MenuScene
        SceneManager.LoadScene("MenuScene");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!"); // This works in builds, but not in the editor
    }
}
