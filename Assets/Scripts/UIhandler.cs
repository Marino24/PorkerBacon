using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIhandler : MonoBehaviour
{
    [Header("References")]
    public GameObject inventory;
    public GameObject menu;
    public GameObject monologue;
    public TMP_Text monologueText;
    public GameObject monologueBG;
    public Vector3 monologueBGOffset;

    public GameObject hoveringInfo;
    public TMPro.TMP_Text hoveringText;
    public Vector3 hoveringOffset;

    public Player player;
    public static UIhandler instance;


    [Header("Conversation")]
    public GameObject conversationStage;


    [Header("Menu")]
    public GameObject MenuStage;


    public static bool GameisPaused = false;
    public void Resume()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        GameisPaused = false;
    }
    void Pause()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
        GameisPaused = true;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void Quit()
    {
        Application.Quit();
    }

    void Awake()
    {
        Resume();  //THIS IS IMPORTANT
        menu.SetActive(false);
    }

    private void OnEnable()
    {
        instance = this;
        ConvController.startConvo += x => StartConversation();
        ConvController.endConvo += EndConversation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameisPaused) Resume(); else Pause();
        }

    }


    public void StartConversation()
    {

        conversationStage.SetActive(true);
        inventory.SetActive(false);
        monologueText.text = ""; monologue.SetActive(false);
        player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero; player.enabled = false;

    }

    public void EndConversation()
    {
        conversationStage.SetActive(false);

        inventory.SetActive(true);
        monologue.SetActive(true);
        player.enabled = true;

    }

    public void HoverOn(string info)
    {
        hoveringInfo.SetActive(true);
        hoveringText.text = info;
        hoveringInfo.transform.position = Input.mousePosition + hoveringOffset;
    }

    public void HoverOff()
    {
        hoveringInfo.SetActive(false);
        hoveringText.text = "";
    }

}
