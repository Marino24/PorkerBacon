using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioController : MonoBehaviour
{
    public AudioSource[] audioSources;

    public static Action<string> musicPlay;
    public static Action<string> musicStop;

    void OnEnable()
    {
        musicPlay += UnMuteMusic;
        musicStop += MuteMusic;
    }

    void OnDisable()
    {
        musicPlay -= UnMuteMusic;
        musicStop -= MuteMusic;
    }

    public static IEnumerator Fade(AudioSource audioSource, float duration, float targetVolume)
    {
        Debug.Log("faaadiing");
        audioSource.mute = !audioSource.mute;
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    public void UnMuteMusic(string musicName)
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].gameObject.name == musicName)
            {
                StartCoroutine(Fade(audioSources[i], 8f, 1));
                break;
            }
        }
    }

    public void MuteMusic(string musicName)
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].gameObject.name == musicName)
            {
                StartCoroutine(Fade(audioSources[i], 4f, 0));
                break;
            }
        }
    }



}
