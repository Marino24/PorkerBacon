using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockNewOption : MonoBehaviour
{
    public Conversation root;
    public Conversation.OptionData firstItem; public string firstItemReq;
    public Conversation.OptionData firstRecipe; public string firstRecipeReq;

    private void OnEnable()
    {
        Object.pickedAnItem += FirstItem;
    }
    public void FirstItem(Sprite x, string y)
    {
        root.toBeAddedOptionDataSet.Add(firstItem, firstItemReq);
        Object.pickedAnItem -= FirstItem;
    }
    private bool a = true;
    public void FirstRecipe()
    {
        if (a)
        {
            root.toBeAddedOptionDataSet.Add(firstRecipe, firstRecipeReq);
            a = false;
        }
    }


}
