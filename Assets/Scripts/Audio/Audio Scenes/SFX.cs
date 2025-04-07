using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour, IAudioObserver
{
    [SerializeField] private AudioClip buttonStartSFX;
    [SerializeField] private AudioClip buttonexitSFX;

    [SerializeField] private AudioClip BGM_Inicial;




 

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
        SFX_Driver.Instance.PlaySound(BGM_Inicial);
    }



    
    public void onClickStart()
    {
        SFX_Driver.Instance.StopSound();
        OnSoundPlayed(buttonStartSFX);
       



    }

    public void onClickExit()
    {
        SFX_Driver.Instance.StopSound();
        OnSoundPlayed(buttonexitSFX);
       
    }

}
