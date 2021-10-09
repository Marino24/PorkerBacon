using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemActions : MonoBehaviour
{
    public ConvController convController;
    public Conversation.OptionData option;
    public void Hookline(NPC npc)
    {
        /*
        if (convController.currentConv.optionDataSet.Contains(option) || convController.currentConv.toBeAddedOptionDataSet.ContainsKey(option))
        {
            convController.selectedOption = option;
            convController.isOptionSelected = true;
            ConvController.startConvo?.Invoke(npc.conversation);
        }
        */
    }

}
