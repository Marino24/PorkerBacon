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

    private bool isOptionSelected; private Conversation.OptionData selectedOption; private bool firstTimeSpeaking;
    private int responseIndex;

    void Awake()
    {
        SetUpOptionButtons();
        cam = Camera.main;
        textWritter = cam.GetComponent<TextWritter>();
    }
    private void OnEnable()
    {
        startConvo += ConvoStarted;
    }

    void OnDisable()
    {
        startConvo = null;
        endConvo = null;
        introOver = null;
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
        //first line dialogue?
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
        ShowWhoIsSpeaking("Porker", selectedOption.option, selectedOption.optionAudio);
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

    #region CharName>Sprite(for revision)
    //this whole thing does not need to be here, but it is for now
    public Sprite[] charSprites = new Sprite[8]; public Sprite empty;
    Sprite GetSpriteOfCharacter(Conversation.Character x)
    {

        return x switch
        {
            Conversation.Character.Noone => empty,
            Conversation.Character.Porker => charSprites[0],
            Conversation.Character.Hamilton => charSprites[1],
            Conversation.Character.Muhler => charSprites[2],
            Conversation.Character.Hannah => charSprites[3],
            Conversation.Character.Stan => charSprites[4],
            Conversation.Character.Lambdon => charSprites[5],
            Conversation.Character.Holy => charSprites[6],
            Conversation.Character.Shepherd => charSprites[7],
            _ => empty
        };
    }
    #endregion
    public void ConvoStarted(Conversation convo)
    {
        currentConv = convo;

        //set both sprites to characters talking
        leftChar.sprite = GetSpriteOfCharacter(currentConv.leftChar);
        rightChar.sprite = GetSpriteOfCharacter(currentConv.rightChar);

        isConvoEnded = false; responseIndex = 0; textCharName.text = " ";

        if (currentConv.NarrativeDataSet.Count == 0) isNarrConvo = false; else isNarrConvo = true;

        if (isNarrConvo)
        {
            AdvanceConvo(currentConv.NarrativeDataSet.Count);
        }

        if (!isNarrConvo)
        {
            if (firstTimeSpeaking)
            {
                leftCharBg.gameObject.SetActive(true);
                rightCharBg.gameObject.SetActive(true);
                AudioController.musicPlay?.Invoke(currentConv.leftChar.ToString());
                AudioController.musicPlay?.Invoke(currentConv.rightChar.ToString());
            }
            firstTimeSpeaking = false;
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

    void ShowWhoIsSpeaking(string charName, string spokenText, AudioClip clip)
    {
        textCharName.text = charName;
        textWritter.ColorText(charName, textRoom);
        textWritter.ColorText(charName, textCharName);
        textWritter.Write(spokenText, textRoom, true);
        AudioController.playClip?.Invoke(clip);
        //add code for enlargment/animation here

    }
    void AdvanceConvo(int numOfResponses)
    {

        //go through each text block/response
        if (responseIndex < numOfResponses)
        {
            string spokenText;
            AudioClip spokenClip;

            if (isNarrConvo)
            {

                Conversation.Talking talk = currentConv.NarrativeDataSet[responseIndex].talking;

                spokenText = currentConv.NarrativeDataSet[responseIndex].textSet;
                spokenClip = currentConv.NarrativeDataSet[responseIndex].textSetAudio;


                if (talk == Conversation.Talking.left)
                {
                    if (!leftSpoke) FirstWords(currentConv.leftChar.ToString(), leftCharBg, ref leftSpoke);

                    ShowWhoIsSpeaking(currentConv.leftChar.ToString(), spokenText, spokenClip);
                }
                if (talk == Conversation.Talking.right)
                {
                    if (!rightSpoke) FirstWords(currentConv.rightChar.ToString(), rightCharBg, ref rightSpoke);

                    ShowWhoIsSpeaking(currentConv.rightChar.ToString(), spokenText, spokenClip);
                }
            }
            else
            {
                spokenText = selectedOption.responses[responseIndex];
                spokenClip = selectedOption.responsesAudio[responseIndex];

                if (currentConv.leftChar != Conversation.Character.Porker)
                    ShowWhoIsSpeaking(currentConv.leftChar.ToString(), spokenText, spokenClip);
                else
                    ShowWhoIsSpeaking(currentConv.rightChar.ToString(), spokenText, spokenClip);
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

                //we leave the conversation (option stays)
                if (selectedOption.isStickyOption)
                {
                    EndCurrentConvo();
                    npc.conversation = currentConv;
                }
                else
                {
                    //we leave, option gets removed
                    if (selectedOption.isExitOption)
                    {
                        currentConv.optionDataSet.Remove(selectedOption);
                        EndCurrentConvo();
                        npc.conversation = currentConv;
                    }
                    else
                    {
                        //we continue on as normal

                        Conversation temp = selectedOption.nextConvo;
                        currentConv.optionDataSet.Remove(selectedOption);
                        //gets next if any
                        if (temp != null)
                        {
                            currentConv = temp;
                        }
                        ConvoStarted(currentConv);
                    }

                }
            }
        }
    }
    void EndCurrentConvo()
    {
        textRoom.text = "";
        firstTimeSpeaking = true;

        leftCharBg.gameObject.SetActive(false); leftSpoke = false;
        AudioController.musicStop?.Invoke(currentConv.leftChar.ToString());

        if (currentConv.rightChar != Conversation.Character.Noone)
        {
            rightCharBg.gameObject.SetActive(false); rightSpoke = false;
            AudioController.musicStop?.Invoke(currentConv.rightChar.ToString());
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
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) && !UIhandler.GameisPaused)
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