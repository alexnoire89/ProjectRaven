using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerYellBoss : MonoBehaviour, IAudioObserver
{

    [SerializeField] private AudioClip yellBossSFX;

    private bool playOnce = true;
    private bool isActivated = false;





   

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


    void Update()
    {
        if (isActivated && playOnce)
        {
            OnSoundPlayed(yellBossSFX);
            playOnce = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Si el PJ se para en el mosaico destruye la base que mantiene la bola
        if (collision.gameObject.tag == "Player")
        {
            isActivated = true;


        }



    }

}
