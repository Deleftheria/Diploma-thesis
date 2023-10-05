using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable()]
public struct SoundParameters
{
    [Range(0, 1)]
    public float Volume;
    [Range(-3, 3)]
    public float Pitch;
    public bool Loop;
}
[System.Serializable()]
public class Sound
{
    public string name;
    public AudioClip clip;
    public SoundParameters parameters;

    [HideInInspector]
    public AudioSource source;

    public void Play()
    {
        source.clip = clip;

        source.volume = parameters.Volume;
        source.pitch = parameters.Pitch;
        source.loop = parameters.Loop;

        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}

public class QuizAudioManager : MonoBehaviour
{
    public static QuizAudioManager instance;

    [SerializeField] Sound[] sounds;
    [SerializeField] AudioSource sourcePrefab;

    [SerializeField] string startupTrack;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        InitSounds();
    }

    void Start()
    {
        if (string.IsNullOrEmpty(startupTrack) != true)
        {
            PlaySound(startupTrack);
        }
    }

    void InitSounds()
    {
        foreach (var sound in sounds)
        {
            AudioSource source = (AudioSource)Instantiate(sourcePrefab, gameObject.transform);
            source.name = sound.name;

            sound.source = source;
        }
    }

    public void PlaySound(string name)
    {
        Sound sound = GetSound(name);
        if (sound != null)
        {
            sound.Play();
        }
        else
        {
            Debug.LogWarning("Sound by the name " + name + " is not found!");
        }
    }

    public void StopSound(string name)
    {
        Sound sound = GetSound(name);
        if (sound != null)
        {
            sound.Stop();
        }
        else
        {
            Debug.LogWarning("Sound by the name " + name + " is not found!");
        }
    }

    Sound GetSound(string name)
    {
        foreach (var sound in sounds)
        {
            if (sound.name == name)
            {
                return sound;
            }
        }

        return null;
    }
}
