using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public UseItem useItem;

    void Awake()
    {
        useItem = Camera.main.GetComponent<UseItem>();
    }

    [Header("Data")]
    public string correctItem;

    public ConvController convCtrl;
    public Conversation conversation;

    void OnMouseOver()
    {
        MouseUi.hooveringItem?.Invoke("npc");
    }

    void OnMouseExit()
    {
        MouseUi.hooveringItem?.Invoke("idle");
    }

    void OnMouseDown()
    {
        /*
        if (useItem.itemUsed == "")
        {
            //default text for just interacting
            textRoom.text = dialogue;

        }
        else if (useItem.itemUsed == correctItem)
        {
            textRoom.text = "Thats what i needed, thank you!";
            useItem.item.sprite = null;
            useItem.item.gameObject.name = "Empty";
        }
        else
        {
            textRoom.text = "Whats that?";
        }

        */
    }

    public void StartConvoWithMe()
    {
        convCtrl.npc = this;
        ConvController.startConvo?.Invoke(conversation);
    }
}
