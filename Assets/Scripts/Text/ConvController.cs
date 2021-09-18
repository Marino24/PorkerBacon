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


    [Header("References")]
    public Conversation currentConv;
    private GameObject[] buttons = new GameObject[10]; /*how many are max*/  public GameObject OptionButton;
    public GameObject MusicManager;


    [Header("References - UI")]
    public TMP_Text textRoom;
    public TMP_Text textCharName;
    public RectTransform convoOptions; private GridLayoutGroup gridLayout;
    public Image leftChar; public Image leftCharBg; private bool leftSpoke;
    public Image rightChar; public Image rightCharBg; private bool rightSpoke;

    //convo var
    private bool isNarrConvo;
    private bool isConvoEnded;

    private bool isOptionSelected; private int optionNumSelected;
    private int convoIndex;

    //scroll var
    private float maxScroll;
    private float scroll = -5;

    void Awake()
    {
        SetUpOptionButtons();
        cam = Camera.main;
        gridLayout = convoOptions.gameObject.GetComponent<GridLayoutGroup>();
        textWritter = cam.GetComponent<TextWritter>();

        startConvo += ConvoStarted;
    }


    public void ConvoStarted(Conversation convo)
    {

        currentConv = convo;

        if (leftChar != null) leftChar.sprite = currentConv.leftChar;
        if (rightChar != null) rightChar.sprite = currentConv.rightChar;

        isConvoEnded = false; convoIndex = 0; textCharName.text = " ";

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
        int elementSize = Mathf.RoundToInt(gridLayout.cellSize.y + gridLayout.spacing.y);
        maxScroll = elementSize * (currentConv.optionDataSet.Count - Mathf.RoundToInt(convoOptions.rect.height / elementSize));
    }
    void OptionClicked()
    {
        isOptionSelected = true;

        //reset buttons & get button num 
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == EventSystem.current.currentSelectedGameObject) optionNumSelected = i;

            buttons[i].SetActive(false);
        }

        //write out your option
        textWritter.Write(currentConv.optionDataSet[optionNumSelected].option, textRoom, true);
    }

    void AdvanceConvo(int amount)
    {

        //go through each text block/response
        if (convoIndex < amount)
        {

            if (isNarrConvo)
            {
                //who is talking
                if (currentConv.NarrativeDataSet[convoIndex].ZeroOrOne == 0)
                {
                    textCharName.text = leftChar.sprite.name.Replace("portrait", "");

                    if (!leftSpoke)
                    {
                        leftCharBg.gameObject.SetActive(true);
                        AudioController.musicPlay?.Invoke(currentConv.leftChar.name);
                        leftSpoke = true;
                    }
                }
                if (currentConv.NarrativeDataSet[convoIndex].ZeroOrOne == 1)
                {
                    textCharName.text = rightChar.sprite.name.Replace("portrait", "");

                    if (!rightSpoke)
                    {
                        rightCharBg.gameObject.SetActive(true);
                        AudioController.musicPlay?.Invoke(currentConv.rightChar.name);
                        rightSpoke = true;
                    }
                }

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

    void CheckForRemovingOptions()
    {
        var removedOptions = currentConv.optionDataSet[optionNumSelected].removedOptions;

        if (removedOptions.Count == 0) return;

        for (int i = 0; i < removedOptions.Count; i++)
        {
            RemoveOptions(i);
        }
    }
    void RemoveOptions(int optionIndex)
    {
        var removedOptionName = currentConv.optionDataSet[optionNumSelected].removedOptions[optionIndex];
        Conversation.OptionData removedOption = FindOptionFromName(removedOptionName);


        if (!currentConv.alreadyRemovedOptionDataSet.Contains(removedOption))
        {
            currentConv.alreadyRemovedOptionDataSet.Add(removedOption);
        }

        if (currentConv.optionDataSet.Contains(removedOption))
        {
            currentConv.optionDataSet.Remove(removedOption);
        }

    }

    Conversation.OptionData FindOptionFromName(string name)
    {
        for (int i = 0; i < currentConv.allOptions.Length; i++)
        {

            if (currentConv.allOptions[i].optionName == name.ToUpper()) return currentConv.allOptions[i];
        }
        return null;
    }

    void CheckForAddingOptions()
    {

        var unlockedOptions = currentConv.optionDataSet[optionNumSelected].unlockedOptions;

        if (unlockedOptions.Count == 0) { return; }

        for (int i = 0; i < unlockedOptions.Count; i++)
        {
            Conversation.OptionData toBeUnlockedOption = FindOptionFromName(unlockedOptions[i]);


            //check if options is to be unlocked
            if (toBeUnlockedOption != null)
            {

                //check if option has a requirement
                if (toBeUnlockedOption.requiredAmount == 1)
                {
                    AddOptions(i);
                }
                else
                {
                    //counter up or add if new
                    if (currentConv.toBeUnlockedOptionsDataSet.ContainsKey(toBeUnlockedOption))
                    {
                        currentConv.toBeUnlockedOptionsDataSet[toBeUnlockedOption]++;
                    }
                    else
                    {
                        currentConv.toBeUnlockedOptionsDataSet.Add(toBeUnlockedOption, 1);
                    }

                    //check if required amount is now good
                    if (currentConv.toBeUnlockedOptionsDataSet[toBeUnlockedOption] == toBeUnlockedOption.requiredAmount)
                    {
                        AddOptions(i);
                    }
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