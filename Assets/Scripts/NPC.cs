using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    private UIhandler uIhandler;
    private UseItem useItem;

    void Awake()
    {
        uIhandler = Camera.main.GetComponent<UIhandler>();
        useItem = Camera.main.GetComponent<UseItem>();
    }

    [Header("Data")]
    public string correctItem;

    public ConvController convCtrl;
    public Conversation conversation;



    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && !useItem.isItemInHand)
        {
            convCtrl.npc = this;

            uIhandler.StartConversation();
            convCtrl.currentConv = conversation;
            convCtrl.ConvoStarted();

        }

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
}
