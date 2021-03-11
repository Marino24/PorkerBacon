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
    public AudioClip sound;
    public string correctItem;

    public ConvController convCtrl;
    public Conversation startingConvo; private Conversation loopingConvo;
    private int counter;

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && !useItem.isItemInHand)
        {
            uIhandler.StartConversation();

            if (counter == 0)
            {
                convCtrl.currentConv = startingConvo;
                loopingConvo = startingConvo.nextConvo;
                counter++;
            }
            else
            {
                convCtrl.currentConv = loopingConvo;
            }
            convCtrl.ConvoStarted(1);
            convCtrl.audioSource.clip = sound;
            convCtrl.audioSource.PlayDelayed(3f);
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
