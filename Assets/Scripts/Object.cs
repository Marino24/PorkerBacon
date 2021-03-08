using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Object : MonoBehaviour
{
    private Camera cam;
    private Sprite itemSprite;
    private UIhandler uIhandler;
    private UseItem useItem;

    private TextWritter textWritter;

    public float off;

    void Awake()
    {
        cam = Camera.main;
        itemSprite = GetComponent<SpriteRenderer>().sprite;
        uIhandler = cam.GetComponent<UIhandler>();
        useItem = cam.GetComponent<UseItem>();
        textWritter = cam.GetComponent<TextWritter>();
    }

    [Header("Data")]
    public bool canMoveHere;

    [Tooltip("Is this an item")]
    public bool canPickUp;

    [Tooltip("This will be for all of the logic with checking")]
    public string objDesc;

    [Tooltip("What item should be used on this")]
    public string correctItem;

    void OnMouseDown()
    {
        textWritter.Write(objDesc, uIhandler.monologueText);


        if (canPickUp)
        {
            cam.GetComponent<Inventory>().ItemStored(itemSprite);
            Destroy(gameObject);
            return;
        }





    }



}
