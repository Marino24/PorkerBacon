using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource previousAudioSource;

    private void Awake() {
        
    }

    public void ChangeMusic(AudioSource newAudioSource){
        newAudioSource.timeSamples = previousAudioSource.timeSamples;
        previousAudioSource.Stop();
        newAudioSource.Play();
        previousAudioSource = newAudioSource;
    }
}
