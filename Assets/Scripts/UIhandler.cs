using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIhandler : MonoBehaviour
{
    [Header("References")]
    public GameObject inventory;
    public GameObject itemsUi;
    public GameObject menu;


    [Header("Inventory")]
    public Image[] items;

    public bool invOpen;

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
    void Awake()
    {
        menu.SetActive(false);
        items = itemsUi.GetComponentsInChildren<Image>();
    }

    void Update()
    {
        //open close inventory manually
        if (Input.GetKeyDown(KeyCode.I))
        {
            invOpen = !invOpen;
            OpenCloseInv();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameisPaused) Resume(); else Pause();
        }

    }
    void OpenCloseInv()
    {
        if (invOpen)
        {
            itemsUi.transform.localPosition = new Vector3(0, 5, 0);
        }
        else
            itemsUi.transform.localPosition = new Vector3(0, -50, 0);
    }

    public void StartConversation()
    {
        invOpen = false;
        conversationStage.SetActive(true);
    }

    public void EndConversation()
    {
        conversationStage.SetActive(false);
    }

    public void ItemStored(Sprite itemSprite)
    {
        //looks for an empty spot and set the item there
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i].sprite = itemSprite;
                break;
            }
        }
    }


}
