using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inspired by sebastian lagues audiomanager script 
// and then taken from kaycaul/sneaks game
// this script just keeps getting worse every time i iterate on it
// help
public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    static AudioSource sfxSource;
    static AudioSource[] musicSources = new AudioSource[2];
    static int activeMusicSourceIndex = 0;
    static float masterVolume, musicVolume, sfxVolume;
    public static float GetMasterVolume() => masterVolume;
    public static float GetMusicVolume() => musicVolume;
    public static float GetSfxVolume() => sfxVolume;

    static bool musicStopped = false;

    // update the volume of sources, should be called by ui
    public static void SetMasterVolume(float volume) {
        masterVolume = volume;
        SetMusicVolume(musicVolume);
        SetSfxVolume(sfxVolume);
    }
    
    public static void SetMusicVolume(float volume) {
        musicVolume = volume;
        // only update the active music source
        musicSources[activeMusicSourceIndex].volume = musicVolume * masterVolume;
    }

    public static void SetSfxVolume(float volume) {
        sfxVolume = volume;
        sfxSource.volume = sfxVolume * masterVolume;
    }

    // plays a sound just once
    public static void PlaySound(AudioClip clip) {
        if (clip == null) {
            Debug.LogError("Tried to play null sound clip!");
            return;
        }
        sfxSource.PlayOneShot(clip);
    }

    // starts a new song and stops the previous one, by fading it out
    public static void PlayMusic(AudioClip clip, float fadeDuration = 1) {
        musicStopped = false;
        if (clip == null) {
            Debug.LogError("Tried to play null music clip!");
            return;
        }
        
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();

        instance.StartCoroutine(CrossFade(
            fadeDuration, 
            musicSources[activeMusicSourceIndex], 
            musicSources[1 - activeMusicSourceIndex],
            musicVolume * masterVolume
            ));
    }

    // fade between two audio sources
    static IEnumerator CrossFade(float duration, AudioSource sourceTo, AudioSource sourceFrom, float maxVolume) {
        float percent = 0;
        while (percent < 1) {
            percent += Time.unscaledDeltaTime / duration;
            if (sourceTo is not null) sourceTo.volume = Mathf.Lerp(0, maxVolume, percent);
            if (sourceFrom is not null) sourceFrom.volume = Mathf.Lerp(maxVolume, 0, percent);
            yield return null;
        }
        // stupid hack
        if (musicStopped) musicSources[activeMusicSourceIndex].volume = 0;
        else SetMusicVolume(musicVolume);
    }

    private void Awake() {

        // delete or persist, there can be only one
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        // set up music sources, and parent them
        for (int i = 0; i < musicSources.Length; i++) {
            GameObject newSource = new GameObject("Music Source " + i);
            musicSources[i] = newSource.AddComponent<AudioSource>();
            musicSources[i].loop = true;
            newSource.transform.parent = transform;
        }
        // same with sound source
        GameObject newSfxSource = new GameObject("Sfx Source");
        newSfxSource.transform.parent = transform;
        sfxSource = newSfxSource.AddComponent<AudioSource>();

        // temp for volume testing, might keep it tho
        SetMasterVolume(0.8f);
        SetSfxVolume(0.8f);
        SetMusicVolume(0.2f);
        // \temp
    }

    public static void StopMusic() {
        musicStopped = true;
        instance.StartCoroutine(CrossFade(1, null, musicSources[activeMusicSourceIndex], musicVolume * masterVolume));
        musicSources[activeMusicSourceIndex].clip = null; // did this in case it tries to fade out later when the other source fades in idk hopefully it doesnt break
    }
}