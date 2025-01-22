using System.Collections;
using System.Collections.Generic;
using Ommy.Audio;
using Ommy.SaveData;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
    public string policyLink;
    public GameObject soundOn,soundOff;
    public GameObject musicOn,musicOff;
    public GameObject hapticsOn,hapticsOff;
    private void OnEnable() 
    {
        SoundClick(SaveData.Instance.SFX);
        MusicClick(SaveData.Instance.Music);
        HapticsClick(SaveData.Instance.Haptic);
    }
    public void CloseClick()
    {
        gameObject.SetActive(false);
    }
    public void PrivacyPolicyClick()
    {
        Application.OpenURL(policyLink);
    }
    public void SoundClick(bool active)
    {
        SaveData.Instance.SFX = active;
        soundOn.SetActive(active);
        soundOff.SetActive(!active);
        AudioManager.Instance?.SetSFXSetting(active);
    }
    public void MusicClick(bool active)
    {
        SaveData.Instance.Music = active;
        musicOn.SetActive(active);
        musicOff.SetActive(!active);
        AudioManager.Instance?.SetBGSetting(active);
    }
    public void HapticsClick(bool active)
    {
        SaveData.Instance.Haptic = active;
        hapticsOn.SetActive(active);
        hapticsOff.SetActive(!active);
        AudioManager.Instance?.SetHapticSetting(active);
    }
    private void OnDisable() 
    {
        SaveSystem.SaveProgress();
    }
}
