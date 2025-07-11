using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalGate : MonoBehaviour, IAudioObserver
{
    [SerializeField] public float upLimitDoor = 8;
    [SerializeField] public float speedDoor = 2;
    private float positionLimitY;
    private float InitialPositionY;

    [SerializeField] private AudioClip doorSFX;

    bool isOpened = false;
    bool playOnce = true;


   

    void OnDestroy()
    {
        SFX_Driver.Instance.RemoveObserver(this);
    }

    public void OnSoundPlayed(AudioClip audioClip)
    {
        SFX_Driver.Instance.PlaySound(audioClip);
    }


    public void Start()
    {
        InitialPositionY = transform.position.y;
        positionLimitY = transform.position.y + upLimitDoor;
        SFX_Driver.Instance.RegisterObserver(this);
    
}

    private void Update()
    {
        if(isOpened && playOnce)
        {
            OnSoundPlayed(doorSFX);
            playOnce = false;
        }


        if ((isOpened) && (transform.position.y < positionLimitY))
        {
            transform.Translate(0, Time.deltaTime * speedDoor, 0);
        }
    }

    public void ResetDoor()
    {
        isOpened = false;
        playOnce = true;
        transform.position = new Vector3(transform.position.x, InitialPositionY, transform.position.z);
    }

    public void CloseDoor()
    {
        isOpened = true;
       

    }
}
