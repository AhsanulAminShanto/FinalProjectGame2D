using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoBehaviour
{
    public void PlayGame()
    {
        // Play button click sound
        SoundEffectPlayer.instance.PlayButtonClickSound();

        // Load the game scene
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        // Play button click sound
        SoundEffectPlayer.instance.PlayButtonClickSound();

        // Quit the application
        Application.Quit();
    }
}
