using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicProgressTracker : MonoBehaviour
{

    void Awake()
    {

    }

    private void FirstItemPickedUp()
    {
        AudioController.musicPlay("First item");

    }

}
