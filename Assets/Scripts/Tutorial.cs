using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Tutorial : MonoBehaviour
{
    public Image toolTip;
    public Image toolTip2;

    public Sprite mouse, keyboard, inventory;
    public Conversation introConvo;





    void Awake()
    {
        Object.pickedAnItem += x => ShowInvetoryTip();
        ConvController.introOver += ShowMovementTip;

        //cause object gets destroyed and this works for every object this logic of firsttiming like this doesnt work
        //have to reverse the logic so its here (same for music when first item picked up)
    }

    public void ShowMovementTip(Conversation convo)
    {
        if (convo != introConvo) return;

        toolTip.sprite = keyboard;
        toolTip2.sprite = mouse;
        toolTip.color += new Color(0, 0, 0, 1);
        toolTip2.color += new Color(0, 0, 0, 1);
        StartCoroutine(FadeOut(toolTip));
        StartCoroutine(FadeOut(toolTip2));

        ConvController.introOver -= ShowMovementTip;
    }

    public void ShowInvetoryTip()
    {
        toolTip.sprite = inventory;
        toolTip.color += new Color(0, 0, 0, 1);
        toolTip2.color -= new Color(0, 0, 0, 1);
        StartCoroutine(FadeOut(toolTip));

        Object.pickedAnItem -= x => ShowInvetoryTip();
    }

    //same as in Inventory script
    private IEnumerator FadeOut(Image obj)
    {
        float x = 0.1f; float y = 1f;
        while (y > 0.1f)
        {
            y -= 0.1f;
            yield return new WaitForSeconds(y);
        }
        while (obj.color.a > 0.01f)
        {
            obj.color -= new Color(0, 0, 0, 0.01f);
            if (x > 0.03) x -= 0.01f;
            yield return new WaitForSeconds(x);
        }
    }

}
