using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWater : MonoBehaviour
{
    public Animator bowlAnimator;
    private Object item;

    private void Awake()
    {
        item = gameObject.GetComponent<Object>();
    }

    private void OnEnable()
    {
        Object.pickedAnItem += Activate;
    }
    private void Activate(Sprite correctItem, string x)
    {
        if (correctItem == item.itemSpriteUI)
        {
            bowlAnimator.SetBool("filling",true);
            //some warnings happen idk what they mean
        }
    }
}
