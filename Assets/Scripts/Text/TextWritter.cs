using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextWritter : MonoBehaviour
{
    private int index = 0;
    private float textRevealSpeed; private float textStayTime;
    public static bool textEnded;
    public void Write(string text, TMP_Text UiText, bool convo, bool forMonologue = false, bool forPickingUpItem = false)
    {
        if (forMonologue)
        {
            UIhandler.instance.monologueBG.SetActive(true);
        }
        if (convo)
        {
            textRevealSpeed = 0.01f;
            textStayTime = 0.1f;
        }
        else
        {
            textRevealSpeed = 0.08f;
            textStayTime = 1f;
        }
        index = 0;
        textEnded = false;
        StopAllCoroutines();
        StartCoroutine(WriteC(text, UiText, convo, forMonologue, forPickingUpItem));
    }


    private IEnumerator WriteC(string text, TMP_Text UiText, bool convo, bool forMonologue = false, bool forPickingUpItem = false)
    {
        while (index < text.Length && !textEnded)
        {
            index++;
            UiText.text = text.Substring(0, index);
            UiText.text += "<color=#00000000>" + text.Substring(index) + "</color>";

            yield return new WaitForSeconds(textRevealSpeed);
        }
        index = 0;

        if (convo)
        {
            UiText.text = text;
        }

        yield return new WaitForSeconds(textStayTime); //cant brute force through the text

        textEnded = true;

        if (forMonologue)
        {
            UIhandler.instance.monologueBG.SetActive(false);
        }
        if (forPickingUpItem)
        {
            Inventory.instance.pickingUpAnItem = false;
        }

        if (!convo)
        {
            UiText.text = "";
        }

    }

    public void ColorText(string charName, TMP_Text UiText)
    {
        var color = charName switch
        {
            "Porker" => new Color32(255, 51, 204, 255),
            "Hamilton" => new Color32(0, 51, 204, 255),
            "Muhler" => new Color32(0, 102, 0, 255),
            "Hannah" => new Color32(255, 0, 0, 255),
            "Stan" => new Color32(128, 0, 0, 255),
            "Rusty" => new Color32(255, 204, 0, 255),
            "Lambdon" => new Color32(179, 134, 0, 255),
            "Holy" => new Color32(153, 0, 153, 255),
            "Shepherd" => new Color32(0, 0, 0, 255),
            _ => new Color32(1, 1, 1, 1),
        };
        UiText.color = color;
    }






}
