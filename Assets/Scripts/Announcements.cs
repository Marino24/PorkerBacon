using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Announcements : MonoBehaviour
{
    public TextMeshProUGUI uiText;

    public void ChickenMission()
    {
        //show Mission unlockeds
        uiText.text = "[Mission Unlocked: TALK to the CHICKENS.]";
        StartCoroutine(FadeOut());
        AudioController.PlayTrackFirstTime("ChickenMission");
    }

    private IEnumerator FadeOut()
    {
        int index = 0;
        string text = uiText.text;
        while (index < text.Length)
        {
            index++;
            uiText.text = text.Substring(0, index);
            uiText.text += "<color=#00000000>" + text.Substring(index) + "</color>";

            yield return new WaitForSeconds(0.1f);
        }

        float x = 0.1f;
        while (uiText.color.a > 0.01f)
        {
            uiText.color -= new Color(0, 0, 0, 0.01f);
            if (x > 0.03) x -= 0.01f;
            yield return new WaitForSeconds(x);
        }
        uiText.text = " ";
        uiText.color = new Color(1, 1, 1, 1);

    }
}
