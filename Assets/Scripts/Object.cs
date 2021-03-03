using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Object : MonoBehaviour
{
    private Camera cam;
    private Sprite itemSprite;
    private UseItem useItem;
    private Text textRoom;

    void Awake()
    {
        cam = Camera.main;
        itemSprite = GetComponent<SpriteRenderer>().sprite;
        useItem = cam.GetComponent<UseItem>();
    }

    [Header("Data")]
    public bool canMoveHere;

    [Tooltip("Is this an item")]
    public bool canPickUp;

    [Tooltip("This will be for all of the logic with checking")]
    public string objName;
    public string objDesc;

    [Tooltip("What item should be used on this")]
    public string correctItem;

    void OnMouseDown()
    {
        if (canPickUp)
        {
            cam.GetComponent<UIhandler>().ItemStored(itemSprite);
            Destroy(gameObject);
        }

    }

}
