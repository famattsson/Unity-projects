using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Options : MonoBehaviour {

    public AudioMixer audioMixer;
    public TMP_Dropdown resDropdown;

    Resolution[] resolutions;

	// Use this for initialization
	void Start () {
        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " X " + resolutions[i].height);
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resDropdown.AddOptions(options);
        resDropdown.value = currentResIndex;
        resDropdown.RefreshShownValue();
	}
	public void ChangeMusicVolume (float value)
    {
        audioMixer.SetFloat("MusicVolume", value);
    }

    public void ChangeMasterVolume (float value)
    {
        audioMixer.SetFloat("MasterVolume", value);
    }

    public void ChangeEffectsVolume (float value)
    {
        audioMixer.SetFloat("EffectsVolume", value);
    }

    public void ChangeGraphicsQuality (int index)
    {
        QualitySettings.SetQualityLevel(index);
        Debug.Log(QualitySettings.GetQualityLevel());
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log(Screen.fullScreen);
    }

    public void SetResolution (int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log(Screen.currentResolution);
    } 
}
