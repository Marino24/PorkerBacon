using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{

    private Camera cam;
    private UIhandler uIhandler;
    public ConvController convCtrl;
    public Conversation startingConvo;


    void Awake()
    {
        cam = Camera.main;
        uIhandler = cam.GetComponent<UIhandler>();
    }

    void Start()
    {
        //uIhandler.StartConversation();
        //convCtrl.currentConv = startingConvo;
        //  convCtrl.ConvoStarted(0);
    }


}
