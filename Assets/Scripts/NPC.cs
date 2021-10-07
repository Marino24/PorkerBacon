using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class NPC : MonoBehaviour
{
    public UseItem useItem;
    private ConvController convCtrl;

    public Action<int> triggeringAction;
    public List<UnityEvent> actions = new List<UnityEvent>();

    void Awake()
    {
        useItem = Camera.main.GetComponent<UseItem>();
        convCtrl = Camera.main.GetComponent<ConvController>();
    }

    [Header("Data")]
    public string correctItem;
    public string outOfReachDialogue;
    public Conversation conversation;

    private void OnEnable()
    {
        triggeringAction += FireEvent;
    }

    private void OnDisable()
    {
        triggeringAction -= FireEvent;
    }

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
        Debug.Log(conversation);
        //do we hardcode the 3 lines convo to start, it has to play before the Options?
        ConvController.startConvo?.Invoke(conversation);
    }

    public void FireEvent(int i)
    {
        actions[i].Invoke();
    }
}
