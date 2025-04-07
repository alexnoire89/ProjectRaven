using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IAudioObserver
{
    //variable para la velocidad del personaje
    [SerializeField] public float moveSpeed = 14f;

    //variables para movimiento con colision del enemigo
    [SerializeField] float enemyMoveX = 100.0f;
    [SerializeField] float enemyMoveY = 0f;
    private float fireRate = 0.5f;
    private float specialShieldRate = 3f;
    private float lastFireTime = 0f;
    private float lastSpecialShieldTime = 0f;

    //Variable para cantidad de salto
    public float jumpAmount = 7f;

    //se llama a objeto del animador de sprites
    private Animator animator;

    //Llama al script de los stats
    PlayerStats playerstats;

    //Objeto rigibody
    public Rigidbody2D rb;

    //Booleano para saber si PJ esta sobre el suelo
    bool isGrounded;

    //booleano para saber en que dire va el PJ
    bool right;

    //radio de circulo de chequeo de suelo
    public float groundCheckDistance = 1.5f;

    //LAYER PARA EL SALTO
    public LayerMask groundLayer;


    //AUDIO
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip fireSFX;
    [SerializeField] private AudioClip shieldSFX;




    //public GameObject firePrefab;
    //public GameObject fireShieldPrefab;

    private IShootStrategy normalShootStrategy;
    private IShootStrategy specialShieldStrategy;

    public GameObject shootingController;
    public GameObject shieldController;


   

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
        //llamamos al componente para las animaciones
        animator = GetComponent<Animator>();

        // extraemos el componente para usar luego
        playerstats = GetComponent<PlayerStats>();

        rb = GetComponent<Rigidbody2D>();

        SFX_Driver.Instance.RegisterObserver(this);

        normalShootStrategy = shootingController.GetComponent<Fire>();
        specialShieldStrategy = shieldController.GetComponent<FireShield>();
    } 


    void Update()
    {
        if(playerstats.blockkeys == false)
        {
            Movement();

        }

  
    }



    void Movement()
    {


        //Limitador de disparos
        if (Time.time - lastFireTime >= fireRate)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                
              normalShootStrategy.Shoot(50, transform, right);
              OnSoundPlayed(fireSFX);
              lastFireTime = Time.time;
            }
        }

        if (Time.time - lastSpecialShieldTime >= specialShieldRate)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                //hace 10 de daño por frame
                specialShieldStrategy.Shoot(10, transform, right);
                OnSoundPlayed(shieldSFX);
                lastSpecialShieldTime = Time.time;
            }
        }






        //cosulta si esta tocando el piso. El ultimo valor es el layer a comprobar (suelo)
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        //Si se esta presionando cualquier tecla ejecuta el bucle de asignacion de teclas de movimiento
        if (Input.anyKey)
        {

            //Segmento de SALTO
            //raycast cada vez que salta solamente
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                //salta
                rb.AddForce(new Vector2(0f, jumpAmount), ForceMode2D.Impulse);
                OnSoundPlayed(jumpSFX);
                
            }
         



            float moveDirection = Input.GetAxisRaw("Horizontal"); //obtiene dir de mov
            rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

            //Mueve hacia la derecha al personaje
            if (moveDirection > 0)
            {
                //Si se roto a la izquierda lo vuelve a rotar a la derecha
                GetComponent<SpriteRenderer>().flipX = false;
                //activa la animacion de correr
                animator.SetTrigger("startMove");
                right = true;
            }



            //mueve el PJ a la izquierda
            if (moveDirection < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                animator.SetTrigger("startMove");
                right = false;
            }

        }
        //si no se esta presionando ninguna tecla, activa la animacion iddle
        else animator.SetTrigger("notMove");
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        //Colision con enemigo
        if (collision.gameObject.tag == "Enemy")
        {
           

            animator.SetTrigger("Damage");

            
            //Mueve el PJ
            if (right)
            {

                transform.Translate(Time.deltaTime * -enemyMoveX, enemyMoveY, 0);
                
            }
            else
            {
                transform.Translate(Time.deltaTime * enemyMoveX, enemyMoveY, 0);
               
            }
            
        }

      



    }
}
