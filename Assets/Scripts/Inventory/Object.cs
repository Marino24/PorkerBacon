using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class Object : MonoBehaviour
{
    private Camera cam;
    private Sprite itemSprite;
    private UIhandler uIhandler;
    private UseItem useItem;
    private Player player;
    private TextWritter textWritter;
    private Inventory inventory;
    private AudioController audioController;


    void Awake()
    {
        cam = Camera.main;
        itemSprite = GetComponent<SpriteRenderer>().sprite;
        uIhandler = cam.GetComponent<UIhandler>();
        useItem = cam.GetComponent<UseItem>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        textWritter = cam.GetComponent<TextWritter>();
        inventory = cam.GetComponent<Inventory>();
        audioController = cam.GetComponent<AudioController>();

    }

    public static Action<Sprite> pickedAnItem;
    public static Action<Sprite> usedAnItem;

    [Header("Data")]

    [Tooltip("Is this an item")]
    public bool canPickUp;
    public string objDesc; private string reachDesc;
    public string objName;
    private float reach = 15f; private bool outOfReach;


    [Tooltip("What item should be used on this")]
    public Sprite correctItem;

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        //range check
        if (Vector2.Distance(transform.position, player.transform.position) > reach) outOfReach = true; else outOfReach = false;

        if (canPickUp && outOfReach) reachDesc = player.reachOptions[UnityEngine.Random.Range(0, player.reachOptions.Count)];
        
        textWritter.Write(objDesc + " " + reachDesc, uIhandler.monologueText, false);


        //pickup
        if (canPickUp && !outOfReach)
        {
            if(objName == "MudInteractable")
            {
                Player.instance.DigIt();
            }

            pickedAnItem?.Invoke(itemSprite);

            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

        }

        if (correctItem != null)
        {
            if (useItem.itemInHand.sprite == correctItem && !outOfReach)
            {
                //do stuff
                textWritter.Write("Thats all folks thanks", uIhandler.monologueText, false);
                usedAnItem?.Invoke(useItem.itemInHand.sprite);
            }
        }

    }
}
