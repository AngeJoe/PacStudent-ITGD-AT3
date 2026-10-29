using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;

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
        StartCoroutine(PlayMove());

        audioSource.Play();
        yield return new WaitForSeconds(3);
        audioSource.clip = normalMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public IEnumerator PlayMove() 
    {
        if (isMoving)  
        {
            for(;;)
            {
                if (isPellet)
                    playerAudio.PlayOneShot(pacPellet);
                else if (isPower)
                    playerAudio.PlayOneShot(pacPower);
                else if (isMoving)
                    playerAudio.PlayOneShot(pacMove);
                yield return new WaitForSeconds(pacMove.length + 0.2f);
            }
        }
    }
}
