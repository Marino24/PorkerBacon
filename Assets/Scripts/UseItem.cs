using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UseItem : MonoBehaviour
{
    private Camera cam;
    void Awake()
    {
        cam = Camera.main;
    }

    [Header("ONLY FOR DEBUGGING")]

    [Tooltip("Only for observing, not changing")]
    public string itemUsed;

    [Tooltip("Only for observing, not changing")]
    public Image item;

    private bool isUsing = false;
    private GameObject button;
    public void ButtonPressed()
    {
        button = EventSystem.current.currentSelectedGameObject;
        //if 0, the item get put back into the inventory in that spot
        if (button.transform.childCount != 0)
        {
            //if empty, there is no item there, dont do anything
            if (button.transform.GetChild(0).gameObject.name != "Empty")
            {
                isUsing = !isUsing;

                if (isUsing)
                {
                    item = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Image>();
                    //parent it to the top of the inventory
                    item.gameObject.transform.SetParent(cam.GetComponent<UIhandler>().inventory.transform, true);
                    itemUsed = item.name;
                }
            }
        }
        else StopUsing();
    }

    void Update()
    {
        if (isUsing)
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
        item.gameObject.transform.SetParent(button.transform, true);
        isUsing = false;
        item.transform.localPosition = new Vector3(0, 0, 0);
        itemUsed = "";
        item = null;
    }
}
