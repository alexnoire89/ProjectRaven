using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss_Skelleton : Enemy, IAudioObserver
{
   
    [SerializeField] int bossHP;
    private Animator animator;
    public static event Action<int> OnScoreBossChanged;
    public static event Action OnVictory;

    [SerializeField] private AudioClip bossDeathSFX;
    [SerializeField] private AudioClip hitSFX;
    float deathTime;
    bool bossDestroyed = false;



    [Header("Attack")]
    [SerializeField] private Transform driverAttack;
    [SerializeField] private Transform driverAttackLeft;
    [SerializeField] private float attackRadio;
    [SerializeField] private int attackDamage = 30;



    void Start()
    {
        bossHP = data.BaseHealth;
   

    }

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        SFX_Driver.Instance.RegisterObserver(this);
    }


    private void Update()
    {






        if (bossDestroyed)
        {

            deathTime += Time.deltaTime;

            if (deathTime > 5)
            {

                OnVictory?.Invoke();


                Destroy(gameObject);

            }

        }
        else
        {
            base.Patrol();
        }

    }

    public void Attack()
    {
        //Crea un circulo y si el player esta dentro es dañado
        Collider2D[] objectsRight = Physics2D.OverlapCircleAll(driverAttack.position, attackRadio);
        Collider2D[] objectsLeft = Physics2D.OverlapCircleAll(driverAttackLeft.position, attackRadio);

        //recorro los objetos y si alguno tiene la etiqueta de player, acciono el daño
        foreach (Collider2D colision in objectsRight)
        {
            if (colision.CompareTag("Player"))
            {
                colision.GetComponent<PlayerStats>().GetDamaged(attackDamage);
                colision.GetComponent<PlayerController>().GetComponent<Animator>().SetTrigger("Damage");
            }
        }

        foreach (Collider2D colision in objectsLeft)
        {
            if (colision.CompareTag("Player"))
            {
                colision.GetComponent<PlayerStats>().GetDamaged(attackDamage);
                colision.GetComponent<PlayerController>().GetComponent<Animator>().SetTrigger("Damage");
            }
        }
    }


    public void GetDamaged(int damage)
    {
        bossHP -= damage;

        GameObject lc = Instantiate(data.Lifecoin, transform.position, transform.rotation);
        lc.GetComponent<LifeCoin>().healValue = 2;



        if (bossHP <= 0)
        {

            animator.SetTrigger("isDead");


            OnScoreBossChanged?.Invoke(data.Enemy_Score);

            bossDestroyed = true;

          
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;

            OnSoundPlayed(bossDeathSFX);

        }




    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            //SFX_Driver.Instance.PlaySound(enemyDeathSFX);
            animator.SetTrigger("isAttacking");


        }

    }

    //Para matar al enemigo ponemos un trigger ya que la fireball esta en este modo
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "fireball")
        {

            OnSoundPlayed(hitSFX);
            animator.SetTrigger("isHit");




        }




    }

    void OnDestroy()
    {
        SFX_Driver.Instance.RemoveObserver(this);

    }

   
}

