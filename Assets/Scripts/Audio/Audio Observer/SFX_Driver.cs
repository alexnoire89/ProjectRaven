using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Driver : MonoBehaviour
{

    public static SFX_Driver Instance;

    private AudioSource audioSource;


    private List<IAudioObserver> observers = new List<IAudioObserver>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.4f;
    }

    public void RegisterObserver(IAudioObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IAudioObserver observer)
    {
        observers.Remove(observer);
    }

    public void PlaySound(AudioClip sound)
    {
        if(sound != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(sound);
        }
       

    }


    public void StopSound()
    {
        audioSource.Stop();


    }

    public void VolumenValue(float valor)
    {
        audioSource.volume = valor;
    }
}
