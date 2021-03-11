using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{

    public GameObject buttons;
    public GameObject settings;
    public GameObject information;
    public GameObject exitB;


    public void Exit()
    {
        buttons.SetActive(true);
        exitB.SetActive(false);
        information.SetActive(false);
        settings.SetActive(false);
    }
    public void OldGame()
    {
        //load from the save point

    }

    public void NewGame()
    {
        //start fresh
        exitB.SetActive(true);
        SceneManager.LoadScene("Day1", LoadSceneMode.Single);
    }

    public void Settings()
    {
        exitB.SetActive(true);
        settings.SetActive(true);
        buttons.SetActive(false);

    }

    public void Information()
    {
        exitB.SetActive(true);
        information.SetActive(true);
        buttons.SetActive(false);
    }


}
