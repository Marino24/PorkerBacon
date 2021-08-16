using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioController : MonoBehaviour
{
    public AudioSource[] audioSources;

    public static Action<string> musicPlay;
    public static Action<string> musicStop;

    void Awake()
    {
        musicPlay += UnMuteMusic;
        musicStop += MuteMusic;
    }
    private void FadeIn()
    {

    }

    private void FadeOut()
    {

    }

    public void UnMuteMusic(string musicName)
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (audioSources[i].gameObject.name == musicName)
            {
                audioSources[i].mute = false;
                Debug.Log("playing new");
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
                audioSources[i].mute = true;
                Debug.Log("stop playing");

                break;
            }
        }
    }



}
