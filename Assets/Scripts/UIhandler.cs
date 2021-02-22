using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIhandler : MonoBehaviour
{
    [Header("References")]
    public Text textRoom;
    public GameObject inventory;

    [Header("Inventory")]
    public GameObject[] items = new GameObject[4];
    public bool invOpen;

    [Header("Conversation")]
    public GameObject conversationStage;

    [Header("Menu")]
    public GameObject MenuStage;



    void Awake()
    {
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
