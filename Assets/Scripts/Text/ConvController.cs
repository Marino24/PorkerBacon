using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System;


public class ConvController : MonoBehaviour
{

    private Camera cam;
    private TextWritter textWritter;
    [System.NonSerialized]
    public NPC npc;

    public static Action<Conversation> startConvo;
    public static Action endConvo;
    public static Action<Conversation> introOver; //for tutorial


    [Header("References")]
    public Conversation currentConv;
    private GameObject[] buttons = new GameObject[10]; /*how many are max*/  public GameObject OptionButton;
    public GameObject MusicManager;


    [Header("References - UI")]
    public TMP_Text textRoom;
    public TMP_Text textCharName;
    public RectTransform convoOptions;
    public Image leftChar; public Image leftCharBg; private bool leftSpoke;
    public Image rightChar; public Image rightCharBg; private bool rightSpoke;

    //convo var
    private bool isNarrConvo;
    private bool isConvoEnded;

    private bool isOptionSelected; private Conversation.OptionData selectedOption;
    private int responseIndex;

    void Awake()
    {
        SetUpOptionButtons();
        cam = Camera.main;
        textWritter = cam.GetComponent<TextWritter>();

        startConvo += ConvoStarted;
    }


    #region Button Options setup
    void SetUpOptionButtons()
    {

        for (int i = 0; i < buttons.Length; i++)
        {
            GameObject buttonObj = Instantiate(OptionButton, convoOptions.transform.position, Quaternion.identity, convoOptions);
            buttonObj.GetComponent<Button>().onClick.AddListener(OptionClicked);
            buttonObj.SetActive(false);
            buttons[i] = buttonObj;
        }
    }
    void DisplayOptions()
    {

        textWritter.Write(currentConv.firstLine, textRoom, true);
        //set buttons
        for (int i = 0; i < currentConv.optionDataSet.Count; i++)
        {
            buttons[i].SetActive(true);
            if (currentConv.optionDataSet[i].abbOption != "") buttons[i].GetComponentInChildren<TMP_Text>().text = currentConv.optionDataSet[i].abbOption;
            if (currentConv.optionDataSet[i].abbOption == "") buttons[i].GetComponentInChildren<TMP_Text>().text = currentConv.optionDataSet[i].option;
        }
    }
    void OptionClicked()
    {
        isOptionSelected = true;

        //reset buttons & get selected option
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == EventSystem.current.currentSelectedGameObject) selectedOption = currentConv.optionDataSet[i];

            buttons[i].SetActive(false);
        }

        //write out your option
        ShowWhoIsSpeaking("Porker", selectedOption.option);
    }
    #endregion

    #region Adding&Removing options
    Conversation.OptionData FindOptionFromName(string name)
    {
        for (int i = 0; i < currentConv.allOptions.Length; i++)
        {

            if (currentConv.allOptions[i].optionName == name.ToUpper()) return currentConv.allOptions[i];
        }
        return null;
    }
    void CheckForRemovingOptions()
    {
        if (selectedOption.removedOptions.Count == 0) return;

        for (int i = 0; i < selectedOption.removedOptions.Count; i++)
        {
            RemoveOptions(i);
        }
    }
    void RemoveOptions(int optionIndex)
    {
        Conversation.OptionData removedOption = FindOptionFromName(selectedOption.removedOptions[optionIndex]);

        if (!currentConv.alreadyRemovedOptionDataSet.Contains(removedOption))
        {
            currentConv.alreadyRemovedOptionDataSet.Add(removedOption);
        }

        if (currentConv.optionDataSet.Contains(removedOption))
        {
            currentConv.optionDataSet.Remove(removedOption);
        }

    }
    void CheckForAddingOptions()
    {

        if (selectedOption.unlockedOptions.Count == 0) return;

        for (int i = 0; i < selectedOption.unlockedOptions.Count; i++)
        {
            Conversation.OptionData unlockedOption = FindOptionFromName(selectedOption.unlockedOptions[i]);

            //check if options is to be unlocked
            if (unlockedOption != null)
            {
                //check if option has a requirement
                if (unlockedOption.requiredAmount == 1)
                {
                    AddOptions(i);
                }
                else
                {
                    //counter up or add if new
                    if (currentConv.unlockableOptionsDataSet.ContainsKey(unlockedOption))
                    {
                        currentConv.unlockableOptionsDataSet[unlockedOption]++;
                    }
                    else
                    {
                        currentConv.unlockableOptionsDataSet.Add(unlockedOption, 1);
                    }

                    //check if required amount is now good
                    if (currentConv.unlockableOptionsDataSet[unlockedOption] == unlockedOption.requiredAmount)
                    {
                        AddOptions(i);
                    }
                }
            }
        }
    }
    void AddOptions(int optionIndex)
    {
        Conversation.OptionData unlockedOption = FindOptionFromName(selectedOption.unlockedOptions[optionIndex]);

        //check if it wasnt unlocked yet or removed
        if (!currentConv.alreadyUnlockedOptionDataSet.Contains(unlockedOption) && !currentConv.alreadyRemovedOptionDataSet.Contains(unlockedOption))
        {
            //add to both lists +  (check destination?) to add to
            currentConv.optionDataSet.Add(unlockedOption);

            currentConv.alreadyUnlockedOptionDataSet.Add(unlockedOption);
        }
    }
    #endregion

    public void ConvoStarted(Conversation convo)
    {
        currentConv = convo;

        //set both sprites to characters talking
        if (leftChar != null) leftChar.sprite = currentConv.leftChar;
        if (rightChar != null) rightChar.sprite = currentConv.rightChar;

        isConvoEnded = false; responseIndex = 0; textCharName.text = " ";

        if (currentConv.NarrativeDataSet.Count == 0) isNarrConvo = false; else isNarrConvo = true;

        if (isNarrConvo)
        {
            AdvanceConvo(currentConv.NarrativeDataSet.Count);
        }

        if (!isNarrConvo)
        {
            leftCharBg.gameObject.SetActive(true);
            rightCharBg.gameObject.SetActive(true);
            AudioController.musicPlay?.Invoke(currentConv.leftChar.name);
            AudioController.musicPlay?.Invoke(currentConv.rightChar.name);
            DisplayOptions();
        }
    }

    void FirstWords(string charName, Image charBg, ref bool lefRight)
    {
        textCharName.text = charName;

        charBg.gameObject.SetActive(true);
        AudioController.musicPlay?.Invoke(charName);
        lefRight = true;
    }

    void ShowWhoIsSpeaking(string charName, string spokenText)
    {
        textCharName.text = charName;
        textWritter.ColorText(charName, textRoom);
        textWritter.ColorText(charName, textCharName);
        textWritter.Write(spokenText, textRoom, true);
        //add code for enlargment/animation here

    }
    void AdvanceConvo(int numOfResponses)
    {

        //go through each text block/response
        if (responseIndex < numOfResponses)
        {
            string whoIsSpeaking; string spokenText;

            if (isNarrConvo)
            {
                //who is talking       0 - left      1 - right
                if (currentConv.NarrativeDataSet[responseIndex].ZeroOrOne == 0) whoIsSpeaking = "left"; else whoIsSpeaking = "right";
                spokenText = currentConv.NarrativeDataSet[responseIndex].textSet;

                if (whoIsSpeaking == "left")
                {
                    if (!leftSpoke) FirstWords(leftChar.sprite.name, leftCharBg, ref leftSpoke);

                    ShowWhoIsSpeaking(leftChar.sprite.name, spokenText);
                }
                if (whoIsSpeaking == "right")
                {
                    if (!rightSpoke) FirstWords(rightChar.sprite.name, rightCharBg, ref rightSpoke);

                    ShowWhoIsSpeaking(rightChar.sprite.name, spokenText);
                }
            }
            else
            {
                spokenText = selectedOption.responses[responseIndex];

                if (currentConv.leftChar.name != "Porker")
                    ShowWhoIsSpeaking(currentConv.leftChar.name, spokenText);
                else
                    ShowWhoIsSpeaking(currentConv.rightChar.name, spokenText);
            }

            responseIndex++;
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
                if (!selectedOption.isExitOption)
                {

                    Conversation temp = selectedOption.nextConvo;

                    currentConv.optionDataSet.Remove(selectedOption);

                    if (temp != null)
                    {
                        currentConv = temp;
                    }
                    ConvoStarted(currentConv);
                }
                else
                {
                    EndCurrentConvo();
                    npc.conversation = currentConv;
                }
            }
        }
    }
    void EndCurrentConvo()
    {
        textRoom.text = "";

        leftCharBg.gameObject.SetActive(false); leftSpoke = false;
        AudioController.musicStop?.Invoke(currentConv.leftChar.name);

        if (currentConv.rightChar != null)
        {
            rightCharBg.gameObject.SetActive(false); rightSpoke = false;
            AudioController.musicStop?.Invoke(currentConv.rightChar.name);
        }


        if (currentConv.nextConvo != null)
        {
            currentConv = currentConv.nextConvo;
            ConvoStarted(currentConv);
        }
        else ConvoEnded();
    }
    public void DebugEnd()
    {
        EndCurrentConvo();
    }
    void ConvoEnded()
    {
        endConvo?.Invoke();
        introOver?.Invoke(currentConv);

        isConvoEnded = true;
    }
    void Update()
    {
        //advance convo 
        if (!isConvoEnded && EventSystem.current.IsPointerOverGameObject())
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
                    if (TextWritter.textEnded) AdvanceConvo(selectedOption.responses.Count);
                    else
                        TextWritter.textEnded = true;

                }

            }
        }
    }

}