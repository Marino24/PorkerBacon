using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "New conversation", order = 1)]
public class Conversation : ScriptableObject
{
    [System.Serializable]
    public class OptionData
    {
        public string optionName = " ";        
        
        public int requiredAmount = 1;
        [TextArea]
        public string option;
        [TextArea]
        public List<string> responses = new List<string>();
        public Conversation nextConvo;
        public List<string> unlockedOptions = new List<string>();
        public List<string> removedOptions = new List<string>();

        public bool isExitOption = false;

    }

    [Header("Options")]
    public string firstLine;

    [SerializeField]
    private List<OptionData> OptionDataSet = new List<OptionData>(); //original one
    [System.NonSerialized]
    public List<OptionData> optionDataSet = new List<OptionData>();

    public OptionData[] allOptions;

    private Dictionary<OptionData, int> RequiredOptionsDataSet = new Dictionary<OptionData, int>(); //original one
    public Dictionary<OptionData, int> requiredOptionsDataSet = new Dictionary<OptionData, int>();

    private List<OptionData> AlreadyUnlockedOptionDataSet = new List<OptionData>(); //original one
    [System.NonSerialized]
    public List<OptionData> alreadyUnlockedOptionDataSet = new List<OptionData>(); 

    private List<OptionData> AlreadyRemovedOptionDataSet = new List<OptionData>(); //original one
    [System.NonSerialized]
    public List<OptionData> alreadyRemovedOptionDataSet = new List<OptionData>(); 


    private void OnEnable()
    {
        optionDataSet = new List<OptionData>(OptionDataSet);
        requiredOptionsDataSet = new Dictionary<OptionData, int>(RequiredOptionsDataSet);
        alreadyUnlockedOptionDataSet = new List<OptionData>(AlreadyUnlockedOptionDataSet);
        alreadyRemovedOptionDataSet = new List<OptionData>(AlreadyRemovedOptionDataSet);
    }


    [System.Serializable]
    public class NarrativeData
    {
        [TextArea(2, 5)]
        public string textSet;
        public int ZeroOrOne;
    }
    [Header("Narrative")]
    public List<NarrativeData> NarrativeDataSet = new List<NarrativeData>();

    [Header("Basic")]
    public Sprite left;
    public Sprite right;
    public Conversation nextConvo;
}
