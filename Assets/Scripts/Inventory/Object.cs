using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Object : MonoBehaviour
{
    private Camera cam;
    private Sprite itemSprite;
    private UIhandler uIhandler;
    private UseItem useItem;
    private Player player;
    private TextWritter textWritter;

    void Awake()
    {
        cam = Camera.main;
        itemSprite = GetComponent<SpriteRenderer>().sprite;
        uIhandler = cam.GetComponent<UIhandler>();
        useItem = cam.GetComponent<UseItem>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        textWritter = cam.GetComponent<TextWritter>();
    }

    [Header("Data")]

    [Tooltip("Is this an item")]
    public bool canPickUp;
    public string objDesc; private string reachDesc;
    public List<string> reachOptions = new List<string>(); private float reach = 7f; private bool outOfReach;


    [Tooltip("What item should be used on this")]
    public string correctItem;

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        //range check
        if (Vector2.Distance(transform.position, player.transform.position) > reach) outOfReach = true; else outOfReach = false;

        if (canPickUp && outOfReach) reachDesc = reachOptions[Random.Range(0, reachOptions.Count)];

        textWritter.Write(objDesc + " " + reachDesc, uIhandler.monologueText, false);

        if (canPickUp && !outOfReach)
        {
            cam.GetComponent<Inventory>().ItemStored(itemSprite);
            Destroy(gameObject);
        }




    }


}
