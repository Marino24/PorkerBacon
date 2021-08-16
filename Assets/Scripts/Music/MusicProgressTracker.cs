using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicProgressTracker : MonoBehaviour
{

    void Awake()
    {
        Object.pickedAnItem += FirstItemPickedUp;

    }

    private void FirstItemPickedUp(Sprite parameter)
    {
        AudioController.musicPlay("First item");
        Object.pickedAnItem -= FirstItemPickedUp;
    }

}
