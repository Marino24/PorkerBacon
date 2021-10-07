using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollForever : MonoBehaviour
{
    public float startPos, endPos, speed;

    
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        Debug.Log(rectTransform.anchoredPosition.y);
        if(rectTransform.anchoredPosition.y < endPos)
        {
            rectTransform.Translate(0f,speed*Time.deltaTime,0f);
        } else {
            rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x,startPos);
        }
    }
}
