using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioController : MonoBehaviour
{
    public List<AudioSource> audioSources = new List<AudioSource>();

    public static Action<string> musicPlay;
    public static Action<string> musicStop;

    void OnEnable()
    {
        DontDestroyOnLoad(gameObject);

        musicPlay += UnMuteMusic;
        musicStop += MuteMusic;

        foreach (Transform track in transform)
        {
            audioSources.Add(track.GetComponent<AudioSource>());
            TrackState.Add(track.name, false);
        }

        musicPlay?.Invoke("Room");

    }


    void OnDisable()
    {
        musicPlay = null;
        musicStop = null;
    }

    #region PlayTrack
    public static IEnumerator Fade(AudioSource audioSource, float duration, float targetVolume)
    {
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
        for (int i = 0; i < audioSources.Count; i++)
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
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].gameObject.name == musicName)
            {
                StartCoroutine(Fade(audioSources[i], 4f, 0));
                break;
            }
        }
    }
    #endregion
    public static Dictionary<string, bool> TrackState = new Dictionary<string, bool>();

    public static void PlayTrackFirstTime(string x)
    {
        if (TrackState.ContainsKey(x))
        {
            if (!TrackState[x])
            {
                musicPlay?.Invoke(x);
                TrackState[x] = true;
            }
        }
    }
}
