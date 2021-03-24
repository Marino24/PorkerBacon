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
    public RectTransform convoOptions; private GridLayoutGroup gridLayout;
    public Image left;
    public Image right;


    //unlocked/removed options tracker
    private List<Conversation.OptionData> AlreadyUnlockedOptionDataSet = new List<Conversation.OptionData>();
    private List<Conversation.OptionData> AlreadyRemovedOptionDataSet = new List<Conversation.OptionData>();
    //unlocked requirement tracker
    Dictionary<string, int> requiredOptionsDataSet = new Dictionary<string, int>();

    //convo var
    private bool isNarrConvo;
    private bool isConvoEnded;
    private bool isOptionSelected; private int optionNumSelected; private float timer;
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


    public void ConvoStarted(int cooldown)
    {

        if (left != null) left.sprite = currentConv.left;
        if (right != null) right.sprite = currentConv.right;


        isConvoEnded = false; convoIndex = 0; timer = cooldown;

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
        //go through each text block/response
        if (convoIndex < amount)
        {
            //who is talking

            if (isNarrConvo)
            {
                if (currentConv.NarrativeDataSet[convoIndex].ZeroOrOne == 0) left.gameObject.SetActive(true);
                if (currentConv.NarrativeDataSet[convoIndex].ZeroOrOne == 1) right.gameObject.SetActive(true);

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
                    ConvoStarted(0);
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
        var removedOption = currentConv.optionDataSet[optionNumSelected].removedOptions[optionIndex];


        if (!AlreadyRemovedOptionDataSet.Contains(removedOption.option))
        {
            AlreadyRemovedOptionDataSet.Add(removedOption.option);
        }

        if (currentConv.optionDataSet.Contains(removedOption.option))
        {
            currentConv.optionDataSet.Remove(removedOption.option);
        }

    }
    void CheckForAddingOptions()
    {
        var requiredOption = currentConv.optionDataSet[optionNumSelected].unlockedOptions;
        if (requiredOption.Count == 0) return;

        for (int i = 0; i < requiredOption.Count; i++)
        {
            //check if options is to be unlocked
            if (requiredOption[i] != null)
            {
                //check if option has a requirement
                if (requiredOption[i].requiredAmount != 0)
                {
                    //counter up or add if new
                    if (requiredOptionsDataSet.ContainsKey(requiredOption[i].id))
                    {
                        requiredOptionsDataSet[requiredOption[i].id]++;
                    }
                    else
                    {
                        requiredOptionsDataSet.Add(requiredOption[i].id, 1);
                    }

                    //check if required amount is now good
                    if (requiredOptionsDataSet[requiredOption[i].id] == requiredOption[i].requiredAmount)
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
        var unlockedOption = currentConv.optionDataSet[optionNumSelected].unlockedOptions[optionIndex];

        //check if it wasnt unlocked yet or removed
        if (!AlreadyUnlockedOptionDataSet.Contains(unlockedOption.option) && !AlreadyRemovedOptionDataSet.Contains(unlockedOption.option))
        {
            //add to both lists + check destination to add to
            if (unlockedOption.unlockedOptionLocation == null)
            {
                currentConv.optionDataSet.Add(unlockedOption.option);
            }
            else
            {
                unlockedOption.unlockedOptionLocation.optionDataSet.Add(unlockedOption.option);
            }

            AlreadyUnlockedOptionDataSet.Add(unlockedOption.option);
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
            ConvoStarted(0);
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