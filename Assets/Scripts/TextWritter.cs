using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextWritter : MonoBehaviour
{
    private int index = 0;

    public void Write(string text, TMP_Text UiText)
    {
        index = 0;
        StopAllCoroutines();
        StartCoroutine(WriteC(text, UiText));
    }
    private IEnumerator WriteC(string text, TMP_Text UiText)
    {
        while (index < text.Length)
        {
            index++;
            UiText.text = text.Substring(0, index);
            UiText.text += "<color=#00000000>" + text.Substring(index) + "</color>";

            yield return new WaitForSeconds(0.05f);
        }
        index = 0;

    }

}
