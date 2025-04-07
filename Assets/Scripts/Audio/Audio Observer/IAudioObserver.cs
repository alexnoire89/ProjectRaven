using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioObserver
{
    void OnSoundPlayed(AudioClip audioClip);
}

