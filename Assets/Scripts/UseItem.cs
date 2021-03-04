using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class UseItem : MonoBehaviour
{
    private Camera cam;
    private Inventory inventory;
    public Image itemInHand;

    [Header("Item combos")]
    public List<ItemCombo> combos = new List<ItemCombo>();
    [System.Serializable]
    public class ItemCombo
    {
        public Sprite result;
        public string item1;
        public string item2;
    }

    [HideInInspector]
    public bool isItemInHand = false;
    private Image currentButton; private Image previousButton;


    void Awake()
    {
        cam = Camera.main;
        inventory = cam.GetComponent<Inventory>();
        currentButton = null;
    }

    public void ButtonPressed()
    {
        previousButton = currentButton;
        currentButton = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();

        if (currentButton == previousButton) { StopUsing(); return; }


        if (isItemInHand)
        {
            //move it there
            if (currentButton.sprite == inventory.empty)
            {
                currentButton.sprite = itemInHand.sprite;
                previousButton.sprite = inventory.empty;
                StopUsing();
                return;
            }

            string itemUsed1 = previousButton.sprite.name;
            string itemUsed2 = previousButton.sprite.name;

            //combine them
            for (int i = 0; i < combos.Count; i++)
            {
                if (itemUsed1 == combos[i].item1 || itemUsed1 == combos[i].item2)
                {
                    if (itemUsed2 == combos[i].item1 || itemUsed2 == combos[i].item2)
                    {
                        currentButton.sprite = inventory.empty;
                        previousButton.sprite = inventory.empty;
                        inventory.ItemStored(combos[i].result);
                        StopUsing();
                        return;
                    }
                }
            }

        }

        if (!isItemInHand)
        {
            if (currentButton.sprite == inventory.empty) return;

            isItemInHand = true;
            itemInHand.sprite = currentButton.GetComponent<Image>().sprite;
        }
    }

    void Update()
    {
        if (isItemInHand)
        {
            itemInHand.transform.position = Input.mousePosition;
            //if clicking aynwhere else, put item away
            if (Input.GetMouseButtonDown(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    StopUsing();
                }
            }

        }
    }

    public void StopUsing()
    {
        //reset everything
        isItemInHand = false;
        itemInHand.transform.localPosition = new Vector3(0, -50, 0);
        itemInHand.sprite = null;
        currentButton = null;
    }
}
