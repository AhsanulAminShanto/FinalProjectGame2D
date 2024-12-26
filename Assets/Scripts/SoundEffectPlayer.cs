using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public static SoundEffectPlayer instance;

    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip levelUpSound;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCorrectSound()
    {
        if (correctSound != null)
            audioSource.PlayOneShot(correctSound);
    }

    public void PlayLevelUpSound()
    {
        if (levelUpSound != null)
            audioSource.PlayOneShot(levelUpSound);
    }
}
