using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicProgressTracker : MonoBehaviour
{

    void Awake()
    {
        Object.pickedAnItem += x => FirstItemPickedUp();
    }

    private void FirstItemPickedUp()
    {
        AudioController.musicPlay("First item");
        Object.pickedAnItem -= x => FirstItemPickedUp();
    }

}
