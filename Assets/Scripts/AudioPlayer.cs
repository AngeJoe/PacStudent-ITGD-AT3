using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer inst;
    public AudioSource audioSource;
    public AudioClip startMusic;
    public AudioClip normalMusic;
    public AudioClip pacMove;

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
        for(;;) 
        {
            audioSource.PlayOneShot(pacMove);
            yield return new WaitForSeconds(pacMove.length + 0.2f);
        }
    }
}
