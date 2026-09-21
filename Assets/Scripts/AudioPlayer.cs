using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioPlayer;
    public AudioClip startMusic;
    public AudioClip normalMusic;
  
    IEnumerator Start()
    {
        audioPlayer.Play();
        yield return new WaitForSeconds(3);
        audioPlayer.clip = normalMusic;
        audioPlayer.loop = true;
        audioPlayer.Play();
    }
}
