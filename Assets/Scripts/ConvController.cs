using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ConvController : MonoBehaviour
{
    [Header("References")]

    private UIhandler uIhandler;
    public Conversation currentConv;
    public RectTransform convoOptions; private GridLayoutGroup gridLayout;
    public GameObject[] buttons = new GameObject[10];
    public AudioSource audioSource;


    [Header("References - UI")]
    public Text textRoom;

    [Header("References/Data")]
    public Image left;
    public Image right;


    //unlocked options tracker
    private List<Conversation.OptionData> AlreadyUnlockedOptionDataSet = new List<Conversation.OptionData>();
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
    }
    public void ConvoStarted(int cooldown)
    {
        /*
        left.sprite = currentConv.left;
        right.sprite = currentConv.right;
        */
        isConvoEnded = false; timer = cooldown;

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
        textRoom.text = currentConv.firstLine;
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
        //who is talking
        if (currentConv.left != null)
        {
            left.gameObject.SetActive(true);
            right.gameObject.SetActive(true);
        }

        //go through each text block/response
        if (convoIndex < amount)
        {
            if (isNarrConvo)
            {
                textRoom.text = currentConv.NarrativeDataSet[convoIndex].textSet;
            }
            else
            {
                textRoom.text = currentConv.optionDataSet[optionNumSelected].responses[convoIndex];
            }

            /*
            //set sizes for char that is talking
            if (currentConv.NarrativeDataSet[convoIndex].rightSpeaking)
            {
                right.GetComponent<Transform>().localScale = new Vector3(3, 3, 1);
                left.GetComponent<Transform>().localScale = new Vector3(2, 2, 1);
            }
            else
            {
                left.GetComponent<Transform>().localScale = new Vector3(3, 3, 1);
                right.GetComponent<Transform>().localScale = new Vector3(2, 2, 1);
            }
            */
            convoIndex++;
        }
        else
        {
            //convo is ending...
            if (isNarrConvo)
            {
                EndCurrentConvo(currentConv.nextfile);
            }
            else
            {
                isOptionSelected = false;

                var requiredOption = currentConv.optionDataSet[optionNumSelected].unlockedOption;
                //check if options is to be unlocked
                if (currentConv.optionDataSet[optionNumSelected].unlockedOption != null)
                {
                    //check if option has a requirement
                    if (requiredOption.requiredAmount != 0)
                    {
                        //counter up or add if new
                        if (requiredOptionsDataSet.ContainsKey(requiredOption.id))
                        {
                            requiredOptionsDataSet[requiredOption.id]++;
                        }
                        else
                        {
                            requiredOptionsDataSet.Add(requiredOption.id, 1);
                        }

                        //check if required amount is now good
                        if (requiredOptionsDataSet[requiredOption.id] == requiredOption.requiredAmount)
                        {
                            AddOptions();
                        }

                    }
                    else
                    {
                        AddOptions();
                    }
                }

                EndCurrentConvo(currentConv.optionDataSet[optionNumSelected].nextfile);
                //remove the option  
                currentConv.optionDataSet.RemoveAt(optionNumSelected);
            }
        }
    }
    void AddOptions()
    {
        var unlockedOptionSet = currentConv.optionDataSet[optionNumSelected].unlockedOption;

        for (int i = 0; i < unlockedOptionSet.UnlockedOptionDataSet.Count; i++)
        {
            //check if it wasnt unlocked yet
            if (!AlreadyUnlockedOptionDataSet.Contains(unlockedOptionSet.UnlockedOptionDataSet[i]))
            {
                //add to both lists + check destination to add to
                if (unlockedOptionSet.unlockedOptionLocation == null)
                {
                    currentConv.optionDataSet.Add(unlockedOptionSet.UnlockedOptionDataSet[i]);
                }
                else
                {
                    unlockedOptionSet.unlockedOptionLocation.optionDataSet.Add(unlockedOptionSet.UnlockedOptionDataSet[i]);
                }
                AlreadyUnlockedOptionDataSet.Add(unlockedOptionSet.UnlockedOptionDataSet[i]);
            }
        }
    }

    void EndCurrentConvo(string nextFile)
    {
        textRoom.text = "";
        convoIndex = 0;

        left.gameObject.SetActive(false);
        right.gameObject.SetActive(false);


        if (nextFile != "")
        {
            currentConv = Resources.Load<Conversation>("Text/" + nextFile);
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
                    AdvanceConvo(currentConv.NarrativeDataSet.Count);
                }
            }
            else
            {
                if (isOptionSelected && Input.GetKeyDown(KeyCode.Space))
                {
                    //advance the responses...
                    AdvanceConvo(currentConv.optionDataSet[optionNumSelected].responses.Count);

                }

            }
        }

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