using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void StartTHEGame()
    {
        GetComponent<Animator>().enabled = true;
    }

    public void CloseStartPanel()
    {
        gameObject.SetActive(false);
    }
}
