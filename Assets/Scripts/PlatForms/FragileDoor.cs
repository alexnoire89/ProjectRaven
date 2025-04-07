using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragileDoor : MonoBehaviour, IAudioObserver
{
    [SerializeField] public float doorHP = 200;
    float inHP;
    //se llama a objeto del animador de sprites
    private Animator animator;

    [SerializeField] private AudioClip doorHitSFX;



  

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
         inHP = doorHP;
        //llamamos al componente para las animaciones
        animator = GetComponent<Animator>();
        SFX_Driver.Instance.RegisterObserver(this);
    
}

    // Update is called once per frame
    void Update()
    {
     
        if (inHP < doorHP)
        {
            animator.SetTrigger("isShooting");
        }
        
        if (doorHP <= 0)
        {
            Destroy(gameObject);
        }


    }

    public void TakeDamage(int damage)
    {
        doorHP -= damage;
        OnSoundPlayed(doorHitSFX);
    }


}
