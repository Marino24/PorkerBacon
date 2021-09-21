using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public Image[] items;
    public GameObject itemsDisplay; private RectTransform inventory; public Sprite empty;
    public Image itemStored;
    public bool invOpen;

    void Awake()
    {
        items = itemsDisplay.GetComponentsInChildren<Image>();
        inventory = itemsDisplay.transform.parent.GetComponent<RectTransform>();
        empty = items[0].sprite;

        Object.pickedAnItem += ItemStored;
        Object.pickedAnItem += ShowItemStored;
        Object.usedAnItem += ItemUsed;
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
            inventory.anchoredPosition = new Vector3(0, 5, 0);
        }
        else
            inventory.anchoredPosition = new Vector3(0, -70, 0);
    }

    void ShowItemStored(Sprite itemSprite)
    {
        itemStored.sprite = itemSprite;
        itemStored.color += new Color(0, 0, 0, 1);
        StartCoroutine(FadeOut(itemStored));
    }

    private IEnumerator FadeOut(Image obj)
    {
        float x = 0.1f;
        while (obj.color.a > 0.01f)
        {
            obj.color -= new Color(0, 0, 0, 0.01f);
            if (x > 0.03) x -= 0.01f;
            yield return new WaitForSeconds(x);
        }


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

    public void ItemUsed(Sprite itemSprite)
    {
        //looks for an item and remove it 
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].sprite == itemSprite)
            {
                items[i].sprite = empty;
                break;
            }
        }
    }
}
