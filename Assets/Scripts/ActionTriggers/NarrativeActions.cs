using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NarrativeActions : MonoBehaviour
{
    public ConvController convController;
    public Conversation holy0;
    public GameObject holy;
    public static Action checkForActions;

    private void OnEnable()
    {
        checkForActions += HolyAppears;
    }

    public void HolyAppears()
    {
        if (convController.currentConv == holy0)
        {
            holy.SetActive(true);
            checkForActions -= HolyAppears;
        }
    }
}
