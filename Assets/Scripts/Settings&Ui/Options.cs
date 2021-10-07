using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class Options : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider soundSlider;
    public Slider musicSlider;
    public TMPro.TMP_Dropdown resDropDown;
    public Resolution[] compatibleRes;



    void Start()
    {
        soundSlider.value = PlayerPrefs.GetFloat("SoundVal", 50f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVal", 50f);

        mixer.SetFloat("MusicVol", Mathf.Log10(musicSlider.value) * 20);
        mixer.SetFloat("SoundVol", Mathf.Log10(musicSlider.value) * 20);

        compatibleRes = Screen.resolutions;

        TMPro.TMP_Dropdown.OptionDataList optionDataList = new TMPro.TMP_Dropdown.OptionDataList();
        int count = 0;
        int currentOption = 0;
        foreach (Resolution res in compatibleRes)
        {
            if (res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height && res.refreshRate == Screen.currentResolution.refreshRate)
            {
                //Debug.Log("We came here!" + count.ToString());
                //resDropDown.SetValueWithoutNotify(count);
                currentOption = count;
                //Debug.Log("Current Res: " + res.width.ToString() +"x" + res.height.ToString() +" & "+ res.refreshRate.ToString());
                //Screen.SetResolution(res.width,res.height,FullScreenMode.ExclusiveFullScreen,res.refreshRate);
            }
            TMPro.TMP_Dropdown.OptionData optionData = new TMPro.TMP_Dropdown.OptionData();
            optionData.text = res.width + "x" + res.height + " & " + res.refreshRate + "Hz";
            optionDataList.options.Add(optionData);
            count += 1;
        }

        resDropDown.AddOptions(optionDataList.options);
        resDropDown.value = currentOption;
    }

    public void ChangeSound()
    {
        PlayerPrefs.SetFloat("SoundVal", soundSlider.value);
        mixer.SetFloat("SoundVol", Mathf.Log10(musicSlider.value) * 20);
    }

    public void ChangeMusic()
    {
        PlayerPrefs.SetFloat("MusicVal", musicSlider.value);
        mixer.SetFloat("MusicVol", Mathf.Log10(musicSlider.value) * 20);
    }

    public void ChangeResolution()
    {
        Screen.SetResolution(compatibleRes[resDropDown.value - 1].width, compatibleRes[resDropDown.value - 1].height, FullScreenMode.ExclusiveFullScreen, compatibleRes[resDropDown.value - 1].refreshRate);
    }

}
