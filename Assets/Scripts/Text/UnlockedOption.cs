using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Option", menuName = "New Option", order = 1)]
public class Option : ScriptableObject
{
    public int requiredAmount = 1;
    public Conversation unlockedOptionLocation;
    public Conversation.OptionData option;
}
