using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    private Camera cam;
    private UIhandler uIhandler;
    private UseItem useItem;

    void Awake()
    {
        cam = Camera.main;
        uIhandler = cam.GetComponent<UIhandler>();
        useItem = cam.GetComponent<UseItem>();
    }

    [Header("Data")]
    public bool canMoveHere;
    public string dialogue;
    public string correctItem;

    public ConvController convCtrl;
    public Conversation startingConvo; private Conversation loopingConvo;
    private int counter;

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (counter == 0)
            {
                uIhandler.StartConversation();
                convCtrl.currentConv = startingConvo;
                convCtrl.ConvoStarted(2);
                loopingConvo = Resources.Load<Conversation>("Text/" + startingConvo.nextfile);
                counter++;
            }
            else
            {
                uIhandler.StartConversation();
                convCtrl.currentConv = loopingConvo;
                convCtrl.ConvoStarted(1);
            }
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
