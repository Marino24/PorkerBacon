using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextWritter : MonoBehaviour
{
    private int index = 0;

    public static bool textEnded;
    public void Write(string text, TMP_Text UiText, bool convo)
    {
        index = 0;
        textEnded = false;
        StopAllCoroutines();
        StartCoroutine(WriteC(text, UiText, convo));
    }


    private IEnumerator WriteC(string text, TMP_Text UiText, bool convo)
    {
        while (index < text.Length && !textEnded)
        {
            index++;
            UiText.text = text.Substring(0, index);
            UiText.text += "<color=#00000000>" + text.Substring(index) + "</color>";

            yield return new WaitForSeconds(0.05f);
        }
        index = 0;

        if (convo)
        {
            UiText.text = text;
        }

        yield return new WaitForSeconds(1f);

        textEnded = true;

        if (!convo)
        {
            UiText.text = "";
        }

    }



}
