using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{

    void Update()
    {
        if(Input.anyKeyDown)
        {
            StartTHEGame();
        }
    }
    public void StartTHEGame()
    {
        GetComponent<Animator>().enabled = true;
    }

    public void CloseStartPanel()
    {
        gameObject.SetActive(false);
    }
}
