using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IAudioObserver
{
    //booleanos para el switch
    bool isClosed = false;
    bool isOpen = false;
    bool playOnce = true;

    //Llama al script de la puerta
    Door door;

    private Animator animator;

    [SerializeField] private AudioClip doorSFX;





    void OnDestroy()
    {
        SFX_Driver.Instance.RemoveObserver(this);
    }

    public void OnSoundPlayed(AudioClip audioClip)
    {
        SFX_Driver.Instance.PlaySound(audioClip);
    }




    // Start is called before the first frame update
    void Start()
    {
       // Busca en el objeto el tag puerta y extrae el componente per se
        door = GameObject.FindGameObjectWithTag("Door").GetComponent<Door>();
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
           // SFX_Driver.Instance.PlaySound(gateSFX);
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


