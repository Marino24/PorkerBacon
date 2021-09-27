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
        Object.pickedAnItem += Activate;
    }
    private void Activate(Sprite x)
    {
        if (x == item.itemSpriteUI)
        {
            bowlAnimator.enabled = true;
            bowlAnimator.Play("Filling");
        }
    }
}
