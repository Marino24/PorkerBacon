using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public Image[] items;
    public GameObject itemsDisplay; public Sprite empty;
    public bool invOpen;

    void Awake()
    {
        items = itemsDisplay.GetComponentsInChildren<Image>();
        empty = items[0].sprite;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenCloseInv();
        }
    }
    public void OpenCloseInv()
    {
        invOpen = !invOpen;
        if (invOpen)
        {
            itemsDisplay.transform.parent.transform.localPosition = new Vector3(0, 5, 0);
        }
        else
            itemsDisplay.transform.parent.transform.localPosition = new Vector3(0, -50, 0);
    }

    public void ItemStored(Sprite itemSprite)
    {
        //looks for an empty spot and set the item there
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].sprite == empty)
            {
                items[i].sprite = itemSprite;
                break;
            }
        }
    }
}
