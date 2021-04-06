﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Option", menuName = "New Option", order = 1)]
public class UnlockedOption : ScriptableObject
{
    public int requiredAmount = 1;
    public Conversation unlockedOptionLocation;
    public List<Conversation.OptionData> UnlockedOptionDataSet = new List<Conversation.OptionData>();
}
