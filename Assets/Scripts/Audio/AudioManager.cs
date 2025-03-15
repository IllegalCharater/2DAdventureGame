using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("事件监听")] 
    public PlayAudioEventSO BGMEvent;
    public PlayAudioEventSO FXEvent;
    public FloatEventSO volumeChangeEvent;//通过改变滑动条改变音量
    public VoidEventSO pauseEvent;//通过音量刷新滑动条
    [Header("事件广播")]
    public FloatEventSO syncVolumeEvent;//暂停时同步音量滑动条
    [Header("播放源")]
    public AudioSource BGMSource;
    public AudioSource FXSource;
    public AudioMixer mixer;
    [Header("音量控制")]
    public Slider volumeSlider;
    private void OnEnable()
    {
        FXEvent.OnEventRaised += OnFXEvent;
        BGMEvent.OnEventRaised += OnBGMEvent;
        volumeChangeEvent.OnEventRaised += OnVolumeChangeEvent;
        pauseEvent.OnEventRaised += OnPauseEvent;
        syncVolumeEvent.OnEventRaised += OnSyncVolumeEvent;
    }

    
    
    private void OnDisable()
    {
        FXEvent.OnEventRaised -= OnFXEvent;
        BGMEvent.OnEventRaised -= OnBGMEvent;
        volumeChangeEvent.OnEventRaised -= OnVolumeChangeEvent;
        pauseEvent.OnEventRaised -= OnPauseEvent;
        syncVolumeEvent.OnEventRaised -= OnSyncVolumeEvent;
    }
    
    private void OnSyncVolumeEvent(float vol)
    {
        volumeSlider.value = (vol+80)/100;
    }
    private void OnPauseEvent()
    {
        float volume;
        mixer.GetFloat("MasterVolume", out volume);
        syncVolumeEvent.OnEventRaised(volume);
    }
    private void OnVolumeChangeEvent(float volume)
    {
        var vol = volume * 100 - 80;
        mixer.SetFloat("MasterVolume", vol);
    }

    private void OnBGMEvent(AudioClip clip)
    { 
        BGMSource.clip = clip;
        BGMSource.Play();
    }
    private void OnFXEvent(AudioClip clip)
    {
        FXSource.clip = clip;
        FXSource.Play();
    }
    
    
}
