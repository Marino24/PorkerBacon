using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RollCredits : MonoBehaviour
{
    public float endPos, speed;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (rectTransform.anchoredPosition.y < endPos)
        {
            rectTransform.Translate(0f, speed * Time.deltaTime, 0f);
        }
        else
        {
            LvlLoader.instance.LoadScene(0);
        }
    }
}
