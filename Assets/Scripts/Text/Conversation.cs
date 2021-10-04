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
        public AudioClip optionAudio;
        public string abbOption;
        [TextArea]
        public List<string> responses = new List<string>();
        public List<AudioClip> responsesAudio = new List<AudioClip>();
        public Conversation nextConvo;
        public List<string> unlockedOptions = new List<string>();
        public List<string> removedOptions = new List<string>();

        public bool isExitOption = false;
        public bool isStickyOption = false;

    }

    [Header("Options")]
    public string firstLine;


    [SerializeField]
    private List<OptionData> OptionDataSet = new List<OptionData>(); //original one
    [System.NonSerialized]
    public List<OptionData> optionDataSet = new List<OptionData>();

    public OptionData[] allOptions;

    private Dictionary<OptionData, int> UnlockableOptionsDataSet = new Dictionary<OptionData, int>(); //original one
    public Dictionary<OptionData, int> unlockableOptionsDataSet = new Dictionary<OptionData, int>();

    private List<OptionData> AlreadyUnlockedOptionDataSet = new List<OptionData>(); //original one
    [System.NonSerialized]
    public List<OptionData> alreadyUnlockedOptionDataSet = new List<OptionData>();

    private List<OptionData> AlreadyRemovedOptionDataSet = new List<OptionData>(); //original one
    [System.NonSerialized]
    public List<OptionData> alreadyRemovedOptionDataSet = new List<OptionData>();


    private void OnEnable()
    {
        optionDataSet = new List<OptionData>(OptionDataSet);
        unlockableOptionsDataSet = new Dictionary<OptionData, int>(UnlockableOptionsDataSet);
        alreadyUnlockedOptionDataSet = new List<OptionData>(AlreadyUnlockedOptionDataSet);
        alreadyRemovedOptionDataSet = new List<OptionData>(AlreadyRemovedOptionDataSet);
    }


    [System.Serializable]
    public class NarrativeData
    {
        [TextArea(2, 5)]
        public string textSet;
        public AudioClip textSetAudio;
        public Talking talking;
    }
    public enum Talking
    {
        none,
        left,
        right,
        mid,

    }

    [Header("Narrative")]
    public List<NarrativeData> NarrativeDataSet = new List<NarrativeData>();

    public enum Character
    {
        Noone,
        Porker,
        Hamilton,
        Muhler,
        Hannah,
        Stan,
        Lambdon,
        Holy,
        Shepherd
    }


    [Header("Basic")]
    public Character leftChar;
    public Character rightChar;
    public Conversation nextConvo;
}
