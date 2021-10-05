using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockNewOption : MonoBehaviour
{
    public Conversation root;
    public Conversation.OptionData firstItem; public string[] firstItemReq;
    public Conversation.OptionData firstRecipe; public string[] firstRecipeReq;
    public List<Conversation.OptionData> onHold = new List<Conversation.OptionData>();

    //this is going really bad, another thing which idk if there is a nice solution for
    //like with tutorial hints and special music, we need to trigger this only once (we should join this with Tutorial&Music)
    //but here we need to add a special Option (created here), to the main(root) conversation it belongs to

    //adding it isnt much trouble, but we certainly cant add it at any time, but only when it can logically appear
    /*
        FirstItem - you can pick up items before even talking to Hamilton;
        So then you would be able to pick this new option before even being introduced to him...

        How normal progression options work:
        Each option has a requiered amount (default 1): 
        -B has required amount of 2
        -this means that when option A unlocks option B, it would only gets added if that amount is 1
        -else it gets stored in a Dictionary<Option, int> where its int gets incremented (starting at 1)
        -so when the next time option C unlocks option B again, it now increments Dictionary yet again (now at 2),
        and now the amount is the same as required and option B gets added

        We cant use that there...
        Below would somewhat almost work, if not for the fact that at the time of unlocking yet again some of the options we 
        want to be said before arent used. This would put the option onHold, but we lose the reference to its requirement after
        that and it gets even messier.
    */
    public void FirstItem()
    {
        AddOption(firstItem, firstItemReq);
    }
    public void FirstRecipe()
    {
        AddOption(firstRecipe, firstRecipeReq);
    }
    public void AddOption(Conversation.OptionData option, string[] requriedOption)
    {
        int counter = 0;

        for (int i = 0; i < requriedOption.Length; i++)
        {
            if (counter != i) break;

            foreach (var removed in root.alreadyRemovedOptionDataSet)
            {
                if (removed.optionName == requriedOption[i])
                {
                    counter++;
                    break;
                }
            }
        }
        if (counter == requriedOption.Length)
            root.optionDataSet.Add(option);
        else onHold.Add(option);
    }

    public void ReCheckRequirment()
    {
        foreach (var option in onHold)
        {

        }
    }

}
