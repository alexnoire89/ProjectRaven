using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_GAMEOVER : MonoBehaviour, IAudioObserver
{
    
    [SerializeField] private AudioClip buttonBackSFX;


   

    void OnDestroy()
    {
        SFX_Driver.Instance.RemoveObserver(this);
    }

    public void OnSoundPlayed(AudioClip audioClip)
    {
        SFX_Driver.Instance.PlaySound(audioClip);
    }

    private void Start()
    {
        SFX_Driver.Instance.RegisterObserver(this);
    }


    public void onClickBack()
    {
        SFX_Driver.Instance.StopSound();

       
        OnSoundPlayed(buttonBackSFX);




    }


}
