using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer inst;
    public AudioSource audioSource;
    public AudioSource playerAudio;

    public AudioClip startMusic;
    public AudioClip normalMusic;
    public AudioClip pacMove;
    public AudioClip pacPellet;
    public AudioClip pacPower;

    public bool isMoving = false;
    public bool isPellet = false;
    public bool isPower = false;

    IEnumerator Start()
    {
        inst = this;

        audioSource.Play();
        yield return new WaitForSeconds(3);
        audioSource.clip = normalMusic;
        audioSource.loop = true;
        audioSource.Play();
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
}