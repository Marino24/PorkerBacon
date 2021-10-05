using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour
{

    // Update is called once per frame
    public void OpenGate()
    {
        GetComponent<Animator>().enabled = true;
        GetComponent<AudioSource>().Play();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("GAME OVER");
    }
}
