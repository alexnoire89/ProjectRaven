using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Driver : MonoBehaviour
{

    public static SFX_Driver Instance;

    private AudioSource sfxSource;

    private AudioSource audioSourceA;
    private AudioSource audioSourceB;
    private AudioSource currentSource;
    private AudioSource nextSource;

    [SerializeField] private float volumeLevel = 0.3f;

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

        var sources = GetComponents<AudioSource>();
        if (sources.Length < 3)
        {
            //Si solo tiene uno agregamos otro dinamicamente
            audioSourceA = gameObject.AddComponent<AudioSource>();
            audioSourceB = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();

        }
        else
        {
            audioSourceA = sources[0];
            audioSourceB = sources[1];
            sfxSource = sources[2];
        }

        audioSourceA.loop = true;
        audioSourceB.loop = true;

        audioSourceA.volume = volumeLevel;
        audioSourceB.volume = 0f;

        sfxSource.clip = null;
        sfxSource.volume = volumeLevel;
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;

        currentSource = audioSourceA;
        nextSource = audioSourceB;
    }

    public void CrossfadeMusic(AudioClip newClip, float fadeDuration)
    {
        if (newClip == currentSource.clip) return;

        nextSource.clip = newClip;
        nextSource.Play();
        StopAllCoroutines();
        StartCoroutine(CrossfadeRoutine(fadeDuration));
    }

    private IEnumerator CrossfadeRoutine(float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            currentSource.volume = Mathf.Lerp(volumeLevel, 0f, t);
            nextSource.volume = Mathf.Lerp(0f, volumeLevel, t);
            yield return null;
        }

        currentSource.Stop();

        // Swap
        var temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;
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
            sfxSource.PlayOneShot(sound);
        }
       

    }

    public void PlaySoundWithStop(AudioClip sound)
    {
        if (sound != null)
        {
            StopSound();
            sfxSource.PlayOneShot(sound);
        }


    }


    public void StopSound()
    {
        sfxSource.Stop();


    }

    public void VolumenValue(float valor)
    {
        currentSource.volume = valor;
    }
}
