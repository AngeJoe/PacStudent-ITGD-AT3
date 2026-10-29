using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer inst;
    public AudioSource audioSource;

    public AudioClip startMusic;
    public AudioClip normalMusic;
    public AudioClip scaredMusic;
    public AudioClip deadMusic;
    public AudioClip pacMove;
    public AudioClip pacPellet;
    public AudioClip pacPower;
    public AudioClip cherrySound;
    public AudioClip ghostEatSound;
    public AudioClip wallSound;
    public AudioClip deathSound;

    public bool isMoving = false;
    public bool isPellet = false;
    public bool isPower = false;

    void Awake()
    {
        inst = this;
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "StartScene")
            PlayIntro();
        else
            audioSource.Stop();
    }

    public void PlayMove()
    {
        if (isPellet)
            audioSource.PlayOneShot(pacPellet);
        else if (isPower)
            audioSource.PlayOneShot(pacPower);
        else if (isMoving)
            audioSource.PlayOneShot(pacMove);
    }

    public void PlayStep(bool pellet, bool power)
    {
        isMoving = true;
        isPellet = pellet;
        isPower = power;
        PlayMove();
    }

    void PlayMusic(AudioClip music)
    {
        if (audioSource == null || music == null) return;
        if (audioSource.clip == music && audioSource.isPlaying) return;

        audioSource.Stop();
        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayIntro()
    {
        PlayMusic(startMusic);
    }

    public void PlayNormal()
    {
        PlayMusic(normalMusic);
    }

    public void PlayScared()
    {
        PlayMusic(scaredMusic);
    }

    public void PlayDead()
    {
        PlayMusic(deadMusic);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    
    public void StopMusic()
    {
        audioSource.Stop();
    }
}