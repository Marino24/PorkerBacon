using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "New conversation", order = 1)]
public class Conversation : ScriptableObject
{
    [System.Serializable]
    public class OptionData
    {
        [TextArea]
        public string option;
        [TextArea]
        public List<string> responses = new List<string>();
        public Conversation nextConvo;
        public UnlockedOption unlockedOption;

    }

    [Header("Options")]
    public string firstLine;

    [SerializeField]
    private List<OptionData> OptionDataSet = new List<OptionData>();
    [System.NonSerialized]
    public List<OptionData> optionDataSet = new List<OptionData>();



    private void OnEnable()
    {
        optionDataSet = new List<OptionData>(OptionDataSet);
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
