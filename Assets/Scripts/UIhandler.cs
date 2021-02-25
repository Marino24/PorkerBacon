using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIhandler : MonoBehaviour
{
    [Header("References")]
    public GameObject inventory;
    public GameObject menu;



    [Header("Inventory")]
    public GameObject[] items = new GameObject[4];
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

        //maybe create the inventory here instead of in editor

        //fill up the array with inventory
        for (int i = 0; i < 4; i++)
        {
            items[i] = inventory.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject;
        }
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
            inventory.transform.localPosition = new Vector3(0, -150, 0);
        }
        else
            inventory.transform.localPosition = new Vector3(0, -250, 0);
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

    public void ItemStored(string itemName, Sprite itemSprite)
    {
        //looks for an empty spot and set the item there
        for (int i = 0; i < 8; i++)
        {
            if (items[i].name == "Empty")
            {
                items[i].name = itemName;
                items[i].GetComponent<Image>().sprite = itemSprite;
                break;
            }
        }
    }


}
