using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch1 : MonoBehaviour, IAudioObserver
{
    //booleanos para el switch
    bool isClosed = false;
    bool isOpen = false;
    bool playOnce = true;

    //Llama al script de la puerta
    public Door1 door;
    [SerializeField] private AudioClip doorSFX;
    private Animator animator;


   

    void OnDestroy()
    {
        SFX_Driver.Instance.RemoveObserver(this);
    }

    public void OnSoundPlayed(AudioClip audioClip)
    {
        SFX_Driver.Instance.PlaySound(audioClip);
    }



    void Start()
    {
        // Busca en el objeto el tag puerta y extrae el componente per se
        door = GameObject.FindGameObjectWithTag("Door1").GetComponent<Door1>();
        SFX_Driver.Instance.RegisterObserver(this);
         animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
              if (isOpen && playOnce)
                     {
                      OnSoundPlayed(doorSFX);
                      playOnce = false;
                 }


        // Si el PJ Esta cerca del switch y presiona E se activa.
        if (Input.GetKey(KeyCode.E) && isClosed)
              {
                    animator.SetTrigger("isActivated");
                      isOpen = true;
             }

            if (isOpen)
              {

                 //acciono la clase para abrir la puerta
                 door.OpenDoor();
              }


    }





    private void OnTriggerStay2D(Collider2D collision)
    {
        
        //Si el switch toca al PJ Se activa el bool
        if (collision.gameObject.tag == "Player")
        {
            isClosed = true;
        }

   

    }
}


