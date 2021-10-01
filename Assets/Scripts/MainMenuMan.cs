using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMan : MonoBehaviour
{

    public GameObject menuPanel;
    public GameObject exitButton;
    public GameObject continueButton;


    public GameObject howToPlayPanel;
    public GameObject settingsPanel;

    void Awake()
    {
        Time.timeScale = 1f;
        if(PlayerPrefs.GetInt("firstTimePlay",1) == 0)
        {
            continueButton.SetActive(true);
        }
    }


    public void BackToMainMenu()
    {
        menuPanel.SetActive(true);
        exitButton.SetActive(false);
        howToPlayPanel.SetActive(false);
        settingsPanel.SetActive(false);

    }
    public void ContinueGame()
    {
        //load from the save point
        SceneManager.LoadSceneAsync("Day1", LoadSceneMode.Single);
    }

    public void NewGame()
    {
        //start fresh
        if(PlayerPrefs.GetInt("firstTimePlay",1) == 1)
        {
            PlayerPrefs.SetInt("firstTimePlay",0);
        }
        SceneManager.LoadSceneAsync("Day1", LoadSceneMode.Single);
    }

    public void OpenSettings()
    {
        exitButton.SetActive(true);
        settingsPanel.SetActive(true);
        menuPanel.SetActive(false);

    }

    public void OpenHowToPlay()
    {
        exitButton.SetActive(true);
        howToPlayPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}