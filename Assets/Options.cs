using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Transform allSounds;
    public Transform allMusics;
    public Slider soundSlider;
    public Slider musicSlider;

    public TMPro.TMP_Dropdown resDropDown;

    public Resolution[] compatibleRes;


    void Start()
    {
        soundSlider.value = PlayerPrefs.GetFloat("SoundVal",50f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVal",50f);

        foreach (Transform sound in allSounds)
        {
            sound.GetComponent<AudioSource>().volume = soundSlider.value;
        }

        foreach (Transform music in allMusics)
        {
            music.GetComponent<AudioSource>().volume = musicSlider.value;
        }        




        compatibleRes = Screen.resolutions;
        TMPro.TMP_Dropdown.OptionDataList optionDataList = new TMPro.TMP_Dropdown.OptionDataList();
        int count = 0;
        foreach (Resolution res in compatibleRes)
        {
            if(res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height && res.refreshRate == Screen.currentResolution.refreshRate)
            {
                //Debug.Log("We came here!" + count.ToString());
                //resDropDown.SetValueWithoutNotify(count);
                resDropDown.value = count;
            }
            TMPro.TMP_Dropdown.OptionData optionData = new TMPro.TMP_Dropdown.OptionData();
            optionData.text = res.width + "x" + res.height + " & " + res.refreshRate + "Hz";
            optionDataList.options.Add(optionData);
            count += 1;
        }

        resDropDown.AddOptions(optionDataList.options);
    }

    public void ChangeSound()
    {
        PlayerPrefs.SetFloat("SoundVal",soundSlider.value);
        foreach (Transform sound in allSounds)
        {
            sound.GetComponent<AudioSource>().volume = soundSlider.value;
        }
    }

    public void ChangeMusic()
    {
        PlayerPrefs.SetFloat("MusicVal",musicSlider.value);
        foreach (Transform music in allMusics)
        {
            music.GetComponent<AudioSource>().volume = musicSlider.value;
        }    
    }

    public void ChangeResolution()
    {
        Screen.SetResolution(compatibleRes[resDropDown.value-1].width,compatibleRes[resDropDown.value-1].height,FullScreenMode.ExclusiveFullScreen,compatibleRes[resDropDown.value-1].refreshRate);
    }
}
