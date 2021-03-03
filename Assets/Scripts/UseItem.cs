using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class UseItem : MonoBehaviour
{
    private Camera cam;

    [Header("Item combos")]
    public List<ItemCombo> combos = new List<ItemCombo>();
    [System.Serializable]
    public class ItemCombo
    {
        public string result;
        public string item1;
        public string item2;
    }

    private bool itemInHand = false;
    private GameObject button;

    public Image item;


    void Awake()
    {
        cam = Camera.main;
    }

    public void ButtonPressed()
    {
        button = EventSystem.current.currentSelectedGameObject;

        if (button.transform.GetChild(0).gameObject.name == "Empty")
        {
            return;
        }

        itemInHand = !itemInHand;

        if (itemInHand)
        {
            item.sprite = button.transform.GetChild(0).GetComponent<Image>().sprite;
        }
        else StopUsing();
    }

    void Update()
    {
        if (itemInHand)
        {
            item.transform.position = Input.mousePosition;
            //if clicking aynwhere else, put item away
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.currentSelectedGameObject != button) StopUsing();
            }
        }
    }

    void StopUsing()
    {
        //reset everything
        itemInHand = false;
        item.transform.localPosition = new Vector3(0, 0, 0);
        item.sprite = null;
    }
}
