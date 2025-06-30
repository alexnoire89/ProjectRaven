using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class SFX : MonoBehaviour, IAudioObserver
{
    [SerializeField] private Button startButton;
    [SerializeField] private AudioClip buttonStartSFX;
    //[SerializeField] private AudioClip buttonexitSFX;

    [SerializeField] private AudioClip BGM_Inicial;

    private bool playOnce = false;


 

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
        //SFX_Driver.Instance.PlaySound(BGM_Inicial);
        SFX_Driver.Instance.CrossfadeMusic(BGM_Inicial, 2f);
    }



    
    public void onClickStart()
    {
        if(!playOnce)
        {
            //SFX_Driver.Instance.StopSound();
            OnSoundPlayed(buttonStartSFX);
            playOnce = true;

            startButton.interactable = false;
            //Time.timeScale = 0f;


        }





    }

    //public void onClickExit()
    //{
    //    SFX_Driver.Instance.StopSound();
    //    OnSoundPlayed(buttonexitSFX);
       
    //}

}
