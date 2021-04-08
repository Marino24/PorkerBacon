using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;


public class ConvController : MonoBehaviour
{
    private UIhandler uIhandler;
    private TextWritter textWritter;
    [System.NonSerialized]
    public NPC npc;

    [Header("References")]
    public Conversation currentConv;

    public GameObject[] buttons = new GameObject[10];
    public AudioSource audioSource;


    [Header("References - UI")]
    public TMP_Text textRoom;
    public TMP_Text textCharName;
    public RectTransform convoOptions; private GridLayoutGroup gridLayout;
    public Image left;
    public Image right;

    //convo var
    private bool isNarrConvo;
    private bool isConvoEnded;

    private bool isOptionSelected; private int optionNumSelected; private float timer; //skipping text speed
    private int convoIndex;

    //scroll var
    private float maxScroll;
    private float scroll = -5;

    void Awake()
    {
        uIhandler = Camera.main.GetComponent<UIhandler>();
        gridLayout = convoOptions.gameObject.GetComponent<GridLayoutGroup>();
        textWritter = Camera.main.GetComponent<TextWritter>();
    }


    public void ConvoStarted()
    {

        if (left != null) left.sprite = currentConv.left;
        if (right != null) right.sprite = currentConv.right;


        isConvoEnded = false;

        if (currentConv.NarrativeDataSet.Count == 0)
        {
            //option convo
            isNarrConvo = false;
            DisplayOptions();
        }
        else
        {
            //narrative convo
            isNarrConvo = true;
            AdvanceConvo(currentConv.NarrativeDataSet.Count);
        }

    }


    void DisplayOptions()
    {
        textWritter.Write(currentConv.firstLine, textRoom, true);
        //set buttons
        for (int i = 0; i < currentConv.optionDataSet.Count; i++)
        {
            buttons[i].SetActive(true);
            buttons[i].GetComponentInChildren<Text>().text = currentConv.optionDataSet[i].option;
        }
        int elementSize = Mathf.RoundToInt(gridLayout.cellSize.y + gridLayout.spacing.y);
        maxScroll = elementSize * (currentConv.optionDataSet.Count - Mathf.RoundToInt(convoOptions.rect.height / elementSize));
    }
    public void OptionClicked()
    {
        isOptionSelected = true;
        //get button num from its name 
        optionNumSelected = int.Parse(EventSystem.current.currentSelectedGameObject.name) - 1;

        //reset buttons
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }

        AdvanceConvo(currentConv.optionDataSet[optionNumSelected].responses.Count);

    }
    void AdvanceConvo(int amount)
    {
        timer = 10f; //skipping text speed

        //go through each text block/response
        if (convoIndex < amount)
        {
            //who is talking
            if (currentConv.NarrativeDataSet[convoIndex].ZeroOrOne == 0)
            {
                textCharName.text = left.sprite.name + ":";
                left.gameObject.SetActive(true);
            }
            if (currentConv.NarrativeDataSet[convoIndex].ZeroOrOne == 1)
            {
                textCharName.text = right.sprite.name + ":";
                right.gameObject.SetActive(true);
            }

            if (isNarrConvo)
            {
                textWritter.Write(currentConv.NarrativeDataSet[convoIndex].textSet, textRoom, true);
            }
            else
            {
                textWritter.Write(currentConv.optionDataSet[optionNumSelected].responses[convoIndex], textRoom, true);
            }

            convoIndex++;
        }
        else
        {
            //convo is ending...
            if (isNarrConvo)
            {
                EndCurrentConvo();
            }
            else
            {
                isOptionSelected = false;

                CheckForRemovingOptions();
                CheckForAddingOptions();

                //is not exit button
                if (!currentConv.optionDataSet[optionNumSelected].isExitOption)
                {

                    Conversation temp = currentConv.optionDataSet[optionNumSelected].nextConvo;

                    currentConv.optionDataSet.RemoveAt(optionNumSelected);

                    if (temp != null)
                    {
                        currentConv = temp;
                    }
                    ConvoStarted();
                }
                else
                {
                    EndCurrentConvo();
                    npc.conversation = currentConv;
                    
                }


            }

        }
    }

    void CheckForRemovingOptions()
    {
        var removedOption = currentConv.optionDataSet[optionNumSelected].removedOptions;
        if (removedOption.Count == 0) return;

        for (int i = 0; i < removedOption.Count; i++)
        {
            RemoveOptions(i);
        }
    }
    void RemoveOptions(int optionIndex)
    {
        var removedOptionName = currentConv.optionDataSet[optionNumSelected].removedOptions[optionIndex];
        var removedOption = FindOptionFromName(removedOptionName);


        if (!currentConv.alreadyRemovedOptionDataSet.Contains(removedOption))
        {
            currentConv.alreadyRemovedOptionDataSet.Add(removedOption);
        }

        if (currentConv.optionDataSet.Contains(removedOption))
        {
            currentConv.optionDataSet.Remove(removedOption);
        }

    }

    Conversation.OptionData FindOptionFromName(string name){
        for(int i=0; i< currentConv.allOptions.Length; i++){
            if(currentConv.allOptions[i].GetType().Name == name) return currentConv.allOptions[i];
        }
        return null;
    }

    void CheckForAddingOptions()
    {
        var requiredOptionName= currentConv.optionDataSet[optionNumSelected].unlockedOptions;
        if (requiredOptionName.Count == 0) return;

        for (int i = 0; i < requiredOptionName.Count; i++)
        {
            var requiredOption= FindOptionFromName(requiredOptionName[i]);

            //check if options is to be unlocked
            if (requiredOption != null)
            {
                //check if option has a requirement
                if (requiredOption.requiredAmount != 0)
                {
                    //counter up or add if new
                    if (currentConv.requiredOptionsDataSet.ContainsKey(requiredOption))
                    {
                        currentConv.requiredOptionsDataSet[requiredOption]++;
                    }
                    else
                    {
                        currentConv.requiredOptionsDataSet.Add(requiredOption, 1);
                    }

                    //check if required amount is now good
                    if (currentConv.requiredOptionsDataSet[requiredOption] == requiredOption.requiredAmount)
                    {
                        AddOptions(i);
                    }

                }
                else
                {
                    AddOptions(i);
                }
            }
        }
    }
    void AddOptions(int optionIndex)
    {
        var unlockedOptionName = currentConv.optionDataSet[optionNumSelected].unlockedOptions[optionIndex];
        var unlockedOption = FindOptionFromName(unlockedOptionName);

        //check if it wasnt unlocked yet or removed
        if (!currentConv.alreadyUnlockedOptionDataSet.Contains(unlockedOption) && !currentConv.alreadyRemovedOptionDataSet.Contains(unlockedOption))
        {
            //add to both lists +  (check destination?) to add to
            currentConv.optionDataSet.Add(unlockedOption);
         
            currentConv.alreadyUnlockedOptionDataSet.Add(unlockedOption);
        }
    }

    void EndCurrentConvo()
    {
        textRoom.text = "";

        left.gameObject.SetActive(false);
        right.gameObject.SetActive(false);


        if (currentConv.nextConvo != null)
        {
            currentConv = currentConv.nextConvo;
            ConvoStarted();
        }
        else ConvoEnded();
    }

    void ConvoEnded()
    {
        uIhandler.EndConversation();
        isConvoEnded = true;
    }
    void Update()
    {
        if (timer >= 0) timer -= 0.1f;

        //advance convo 
        if (!isConvoEnded && EventSystem.current.IsPointerOverGameObject() && timer < 0)
        {
            if (isNarrConvo)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //advance the narrative...
                    if (TextWritter.textEnded) AdvanceConvo(currentConv.NarrativeDataSet.Count);
                    else
                        TextWritter.textEnded = true;
                }
            }
            else
            {
                if (isOptionSelected && Input.GetKeyDown(KeyCode.Space))
                {
                    //advance the responses...
                    if (TextWritter.textEnded) AdvanceConvo(currentConv.optionDataSet[optionNumSelected].responses.Count);
                    else
                        TextWritter.textEnded = true;

                }

            }
        }

        Scroll();
    }

    private void Scroll()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (scroll < maxScroll)
            {
                scroll += 1f;
                convoOptions.transform.localPosition = new Vector3(0, scroll + 90, 0);
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (scroll > -5)
            {
                scroll -= 1f;
                convoOptions.transform.localPosition = new Vector3(0, scroll + 90, 0);
            }
        }
    }
}