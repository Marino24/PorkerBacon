using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{

    private Image[] items; private TextMeshProUGUI[] itemNames;
    public GameObject itemsDisplay; public Sprite empty;
    public Image itemStored;
    public bool invOpen;

    public Animation inventoryAnim;

    void Awake()
    {
        items = itemsDisplay.GetComponentsInChildren<Image>();
        empty = items[0].sprite;

        itemNames = itemsDisplay.GetComponentsInChildren<TextMeshProUGUI>();

        Object.pickedAnItem += ItemStored;
        Object.pickedAnItem += ShowItemStored;
        Object.usedAnItem += ItemUsed;
    }

    private void OnEnable()
    {
        Object.pickedAnItem += ItemStored;
        Object.pickedAnItem += ShowItemStored;
        Object.usedAnItem += ItemUsed;
    }

    private void OnDisable()
    {
        /*
        Object.pickedAnItem -= ItemStored;
        Object.pickedAnItem -= ShowItemStored;
        Object.usedAnItem -= ItemUsed;
        */
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetMouseButtonDown(1))
        {
            OpenCloseInv();
        }
    }
    public void OpenCloseInv()
    {
        invOpen = !invOpen;
        if (invOpen)
        {
            inventoryAnim.Play("OpenInventory");
        }
        else
        {
            inventoryAnim.Play("CloseInventory");
        }
    }

    void ShowItemStored(Sprite itemSprite, string itemName)
    {
        //show name aswell?
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

    public void ItemStored(Sprite itemSprite, string itemName)
    {
        //looks for an empty spot and set the item there
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].sprite == empty)
            {
                items[i].sprite = itemSprite;
                itemNames[i].text = itemName;
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
                itemNames[i].text = "";
                break;
            }
        }
    }
}
