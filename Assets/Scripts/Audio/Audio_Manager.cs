using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//Credit to Brackeys youtube tutorial on Audio managers, as the majority of this code and learning how to use it was made by him.


public partial class Audio_Manager : MonoBehaviour
{

    public Sound[] sounds;

    public static Audio_Manager instance;
    //Audio_Manager

    void Awake()
    {

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.origVolume = s.volume;
            s.origPitch = s.pitch;
        }
    }

    // public void Start(){
    //     Play("Main_Area_Theme_Earth");
    // }

    public void OnLevelWasLoaded(){
        foreach (Sound s in sounds)
        {
            Stop(s.name);
            SetVolume(s, s.origVolume);
        }

        if (SceneManager.GetActiveScene().name == "Main_Area"){
            Play("Main_Area_Theme_Earth");
        }
        else if (SceneManager.GetActiveScene().name == "Rocket_Flight"){
            Play("Rocket_Theme_Earth");
        }
        else if (SceneManager.GetActiveScene().name == "Mine_Game"){
            Play("Mine_Theme_Earth");
        }
    }

    public Sound GetSound(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found");
            return null;
        }
        return s;
    }

    public void Play(string name)
    {
        GetAudioSource(name).Play();
    }

    //this addition to the code was made by me, the rest was from Brackeys tutorial
    public void Stop(string name)
    {
        GetAudioSource(name).Stop();
    }

    public bool IsPlaying(string name){
        AudioSource s = GetAudioSource(name);
        if(s == null){
            return false;
        }

        return s.isPlaying;
    }

    public AudioSource GetAudioSource(string name){
        Sound s = GetSound(name);
        if (s == null)
        {
            return null;
        }

        return s.source;
    }

    public AudioSource GetAudioSource(Sound s){
        return s.source;
    }


    public void SetVolume(string name, float volume){
        Sound s = GetSound(name);
        s.volume = volume;
        s.source.volume = s.volume;
    }

    public void SetVolume(Sound s, float volume){
        s.volume = volume;
        s.source.volume = s.volume;
    }

    public float GetVolume(string name){
        Sound s = GetSound(name);
        return s.volume;
    }

    public float GetVolume(Sound s){
        return s.volume;
    }

    public void ResetVolume(string name){
        ResetVolume(GetSound(name));
    }

    public void ResetVolume(Sound s){
        SetVolume(s, s.origVolume);
    }
}