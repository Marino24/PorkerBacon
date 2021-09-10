using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        public Sprite item1;
        public Sprite item2;
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

            Sprite itemUsed1 = currentButton.sprite;
            Sprite itemUsed2 = previousButton.sprite;

            //combine them IMPORTANT: you cant have 2 of the same items in inventory...
            foreach (ItemCombo v in combos)
            {
                if (itemUsed1 == v.item1 || itemUsed1 == v.item2)
                {
                    if (itemUsed2 == v.item1 || itemUsed2 == v.item2)
                    {
                        currentButton.sprite = inventory.empty;
                        previousButton.sprite = inventory.empty;
                        inventory.ItemStored(v.result);
                        StopUsing();
                        return;
                    }
                }
            }
            StopUsing();
            return;

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
        currentButton = null; previousButton = null;
    }

}
