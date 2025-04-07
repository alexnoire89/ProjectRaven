using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour, IAudioObserver
{
    //La vida del PJ
    [SerializeField] public int HP = 100;
    [SerializeField] public LifeBarUI lifebar;
    [SerializeField] public bool KeyA = false;
    public GameObject player;
    private int currentHP;
    bool playerDestroyed = false;
    float currentTime;


    //EVENTO
    public static event Action OnDeath;
    public static event Action<int> OnLifeChanged;


    [SerializeField] private AudioClip deathSFX;
    [SerializeField] private AudioClip damageSFX;

    private Animator animator;
    public bool blockkeys = false;



    void Start()
    {
        //llamamos al componente para las animaciones
        animator = GetComponent<Animator>();
        currentHP = HP;
        SFX_Driver.Instance.RegisterObserver(this);
    }

    void OnDestroy()
    {
        SFX_Driver.Instance.RemoveObserver(this);
    }

    public void OnSoundPlayed(AudioClip audioClip)
    {
        SFX_Driver.Instance.PlaySound(audioClip);
    }

    private void Update()
    {
       


        if (playerDestroyed)
        {
        
                 currentTime += Time.deltaTime;

                 if (currentTime > 5)
                    {
                      OnDeath?.Invoke();

                   currentTime = 0;
                         //Reseteo player para proxima vida

                      playerDestroyed = false;
                      player.GetComponent<CapsuleCollider2D>().enabled = true;
                      player.GetComponent<Rigidbody2D>().gravityScale = 7;
                      blockkeys = false;
                      currentHP = HP;
                      
            }
            
        }
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {

        //colision con la bola indiana jones
        if (collision.gameObject.tag == "Ball")
        {
            GetDamaged(200);
        }

       


        //Colision con enemigo
        if (collision.gameObject.tag == "Enemy")
        {
            GetDamaged(10);

        }


        if (collision.gameObject.tag == "GameOver")
        {
            GetDamaged(200);
        }

      

    }

    



    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Nails")
        {
            GetDamaged(20);


        }


    }





    public void GetDamaged(int damage)
    {
        currentHP -= damage;
        OnLifeChanged?.Invoke(currentHP);

        OnSoundPlayed(damageSFX);


        if (currentHP <= 0)
        {
            OnSoundPlayed(deathSFX);

            //Rompo el collider del Player para que no lo choquen enemigos o trampas y le pongo gravedad 0 para que se quede quieto en el lugar
            player.GetComponent<CapsuleCollider2D>().enabled = false;
            player.GetComponent<Rigidbody2D>().gravityScale = 0;

            //bloqueo las teclas
            blockkeys = true;

            animator.SetTrigger("isDead");

            playerDestroyed = true;
            
        }
       
      


    }

    
    public void GetHeal(int life)
    {
        
        if (currentHP < HP)
        {
            currentHP += life;
            OnLifeChanged?.Invoke(currentHP);
        }
      
    }

 

}
