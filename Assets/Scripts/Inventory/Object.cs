using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class Object : MonoBehaviour
{
    private Camera cam;
    private UIhandler uIhandler;
    private UseItem useItem;
    private Player player;
    private TextWritter textWritter;
    private Inventory inventory;
    private AudioController audioController;


    void Awake()
    {
        cam = Camera.main;
        uIhandler = cam.GetComponent<UIhandler>();
        useItem = cam.GetComponent<UseItem>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        textWritter = cam.GetComponent<TextWritter>();
        inventory = cam.GetComponent<Inventory>();
        audioController = cam.GetComponent<AudioController>();
    }

    public static Action<Sprite, string> pickedAnItem;
    public static Action<Sprite> usedAnItem;

    [Header("Data")]

    [Tooltip("Is this an item")]
    public bool canPickUp;
    public string objDesc; private string reachDesc;
    public Sprite itemSpriteUI;
    public string objName; private string hiddenName;
    public bool nameHiden;
    public float reach = 15f; private bool outOfReach;


    [Tooltip("What item should be used on this")]
    public Sprite correctItem;
    void OnMouseOver()
    {
        if (uIhandler.conversationStage.activeSelf) return;
        if (useItem.isItemInHand)
        {
            MouseUi.hooveringItem?.Invoke("combo");
            return;
        }

        if (canPickUp)
        {
            MouseUi.hooveringItem?.Invoke("item");
        }
        uIhandler.HoverOn(hiddenName ?? objName);

    }

    private void Start()
    {
        if (nameHiden)
        {
            for (int i = 0; i < name.Length; i++)
            {
                hiddenName += "?";
            }
        }
    }

    private void OnDisable()
    {
        pickedAnItem = null;
        usedAnItem = null;
    }


    void OnMouseExit()
    {
        MouseUi.hooveringItem?.Invoke("idle");
        uIhandler.HoverOff();
    }
    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject() || Inventory.instance.pickingUpAnItem) return;


        //range check
        if (Vector2.Distance(transform.position, player.transform.position) > reach) outOfReach = true; else outOfReach = false;

        if (canPickUp && outOfReach) reachDesc = player.reachOptions[UnityEngine.Random.Range(0, player.reachOptions.Count)];
        else reachDesc = "";

        //textWritter.Write(objDesc + " " + reachDesc, uIhandler.monologueText, false, true, true);


        //pickup
        if (canPickUp && !outOfReach && !useItem.isItemInHand)
        {
            inventory.pickingUpAnItem = true;
            PickingUp();
        }

        if (correctItem != null && useItem.itemInHand.sprite == correctItem && !outOfReach)
        {
            //do stuff
            AudioController.PlayTrackFirstTime("EscapeArtist");
            //textWritter.Write("Thats all folks thanks", uIhandler.monologueText, false);
            uIhandler.OpenTheGate();
            usedAnItem?.Invoke(useItem.itemInHand.sprite);
        }
        else
        {
            textWritter.Write(objDesc + " " + reachDesc, uIhandler.monologueText, false, true, true);
        }

    }

    private void PickingUp()
    {
        AudioController.PlayTrackFirstTime("FirstItem");

        if (objName == "Mud")
        {
            Player.instance.DigIt();
        }

        pickedAnItem?.Invoke(itemSpriteUI, objName);

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
