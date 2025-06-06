using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool isMutingBGM = false;
    public bool isMutingSFX = false;

    [Range(0f, 1f)]
    public float BGMVolume = 1f;
    [Range(0f, 1f)]
    public float SFXVolume = 1f;

    public Sound[] BGMSoundList;
    public Sound[] SFXSoundList;

    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        foreach(Sound bgmSound in BGMSoundList)
        {
            bgmSound.audioSource = gameObject.AddComponent<AudioSource>();
            bgmSound.audioSource.clip = bgmSound.audioClip;
            bgmSound.audioSource.volume = bgmSound.volume * BGMVolume;
            bgmSound.audioSource.loop = bgmSound.loop;
            bgmSound.audioSource.mute = bgmSound.mute;
        }
        foreach (Sound sfxSound in SFXSoundList)
        {
            sfxSound.audioSource = gameObject.AddComponent<AudioSource>();
            sfxSound.audioSource.clip = sfxSound.audioClip;
            sfxSound.audioSource.volume = sfxSound.volume * SFXVolume;
            sfxSound.audioSource.loop = sfxSound.loop;
            sfxSound.audioSource.mute = sfxSound.mute;
        }
        PlayTheme();
    }
    public void PlayBGM(string soundName)
    {
        Sound bgm = Array.Find(BGMSoundList, s => s.name == soundName);
        if (bgm != null && !bgm.audioSource.isPlaying) bgm.audioSource.Play();
    }
    public void PlaySFX(string soundName)
    {
        Sound sfx = Array.Find(SFXSoundList, s => s.name == soundName);
        if (sfx != null) sfx.audioSource.PlayOneShot(sfx.audioClip);
    }
    public void StopBGM(string soundName)
    {
        foreach (Sound bgmSound in BGMSoundList)
        {
            if(bgmSound.audioSource.isPlaying && bgmSound.name == soundName) 
                bgmSound.audioSource.Stop();
        }
    }
    public void ToggleBGMState(bool state)
    {
        foreach (Sound bgmSound in BGMSoundList)
        {
            bgmSound.mute = !state;
            bgmSound.audioSource.mute = bgmSound.mute;
        }
        if (state) isMutingBGM = false;
        else isMutingBGM = true;
    }
    public void ToggleSFXState(bool state)
    {
        foreach (Sound sfxSound in SFXSoundList)
        {
            sfxSound.mute = !state;
            sfxSound.audioSource.mute = sfxSound.mute;
        }
        if (state) isMutingSFX = false;
        else isMutingSFX = true;
    }
    //BGM
    public void PlayTheme()
    {
        PlayBGM("Theme");
    }
}
