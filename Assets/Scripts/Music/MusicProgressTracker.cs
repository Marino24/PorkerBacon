using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MusicProgressTracker : MonoBehaviour
{

    //leave this as it is, ill finish/fix it later
    public static Dictionary<string, bool> CheckFor = new Dictionary<string, bool>();

    private int[] nums = new int[8];

    public static void FirstItemPickedUp()
    {
        AudioController.musicPlay?.Invoke("FirstItem");
    }

    private static void CheckCompleted(string x)
    {
        if (CheckFor.ContainsKey(x))
        {
            if (!CheckFor[x])
            {
                AudioController.musicPlay?.Invoke(x);
                CheckFor[x] = true;
            }
        }
    }
    private void Escape()
    {
        AudioController.musicPlay?.Invoke("EscapeArtist");
    }

}
