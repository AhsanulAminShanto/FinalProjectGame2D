using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundEffectPlayer : MonoBehaviour
{
    public static SoundEffectPlayer instance;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip correctSound;      // Sound for correct answer
    [SerializeField] private AudioClip wrongMatchSound;   // Sound for wrong match
    [SerializeField] private AudioClip gameLostSound;     // Sound for losing the game
    [SerializeField] private AudioClip levelUpSound;      // Sound for level up
    [SerializeField] private AudioClip buttonClickSound;  // Sound for button clicks

    [Header("Background Music")]
    [SerializeField] private AudioClip menuBackgroundMusic; // Background music for the menu

    private AudioSource audioSource;
    private AudioSource musicSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        // Main audio source for sound effects
        audioSource = GetComponent<AudioSource>();

        // Separate audio source for background music
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true; // Loop the background music
        musicSource.volume = 0.5f; // Adjust volume as needed

        // Start playing background music if assigned
        PlayMenuBackgroundMusic();

        // Subscribe to scene change event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void PlayCorrectSound()
    {
        if (correctSound != null)
            audioSource.PlayOneShot(correctSound);
    }

    public void PlayWrongMatchSound()
    {
        if (wrongMatchSound != null)
            audioSource.PlayOneShot(wrongMatchSound);
    }

    public void PlayGameLostSound()
    {
        if (gameLostSound != null)
            audioSource.PlayOneShot(gameLostSound);
    }

    public void PlayLevelUpSound()
    {
        if (levelUpSound != null)
            audioSource.PlayOneShot(levelUpSound);
    }

    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
            audioSource.PlayOneShot(buttonClickSound);
    }

    public void PlayMenuBackgroundMusic()
    {
        if (menuBackgroundMusic != null && !musicSource.isPlaying)
        {
            musicSource.clip = menuBackgroundMusic;
            musicSource.Play();
        }
    }

    public void StopMenuBackgroundMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the loaded scene is the menu, start the background music
        if (scene.name == "MenuScene") // Replace "MenuScene" with your menu scene name
        {
            PlayMenuBackgroundMusic();
        }
        else
        {
            StopMenuBackgroundMusic();
        }
    }
}
