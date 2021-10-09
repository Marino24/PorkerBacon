using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupsActions : MonoBehaviour
{
    public Animator bowlAnimator;
    public Sprite stringItem;
    public Sprite nail; public SpriteRenderer crackedFence; public Sprite brokenFence;




    private void OnEnable()
    {
        Object.pickedAnItem += DoThis;
    }
    private void DoThis(Sprite correctItem, string x)
    {
        if (correctItem == stringItem)
        {
            bowlAnimator.SetBool("filling", true);
        }
        Debug.Log(correctItem);
        if (correctItem == nail)
        {
            crackedFence.sprite = brokenFence;
        }
    }
}
