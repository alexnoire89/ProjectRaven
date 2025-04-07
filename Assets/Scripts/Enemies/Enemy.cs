
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class Enemy : MonoBehaviour, IAudioObserver
{
    [Header("Punteros para el rayo de patrulla")]
	[SerializeField] private Transform castPointLeft;
	[SerializeField] private Transform castPointRight;

    public static event Action<int> OnScoreChanged;
	private Rigidbody2D rb;
    private float currentTime;
    private bool right = false;
    private RaycastHit2D ry;
    public EnemyFlyweightData data;
    private int enemyHP;


    private void Awake()
	{
		enemyHP = data.BaseHealth;

    }

	void Start()
	{
        rb = GetComponent<Rigidbody2D>();
        SFX_Driver.Instance.RegisterObserver(this);

    }
    void OnDestroy()
    {
        SFX_Driver.Instance.RemoveObserver(this);
        OnScoreChanged?.Invoke(data.Enemy_Score);
        Destroy(gameObject);
     
    }

    public void TakeDamage(int damage)
	{
        enemyHP -= damage;
		
	}


    private void Update()
	{
		if (data.StatePatrol)
		{
			OnlyPatrol();
		}
		else if (data.StateChase)
        {
            startPatrol();
        }
        else if (data.StateIdle)
        {
            idleInPlace();
        }


        if (enemyHP <= 0)
		{
            GameObject lc = Instantiate(data.Lifecoin, transform.position, transform.rotation);
            lc.GetComponent<LifeCoin>();
            OnDestroy();
         
        }

    }



	internal void ChasingPJ()
	{


		if (transform.position.x < data.Player.position.x)
		{
			//Si el enemigo esta a la izquierda del PJ, lo movemos a la derecha
			rb.velocity = new Vector2(data.MoveSpeed, 0);
			GetComponent<SpriteRenderer>().flipX = !data.XFlip;


		}
		else if (transform.position.x > data.Player.position.x)
		{
			//Si el enemigo esta a la izquierda del PJ, lo movemos a la derecha
			rb.velocity = new Vector2(-data.MoveSpeed, 0);
			GetComponent<SpriteRenderer>().flipX = data.XFlip;

		}
		else
		{
			//Si el enemigo está directamente encima del PJ, se queda quieto
			rb.velocity = Vector2.zero;
		}
	}

	internal void idleInPlace()
    {
		rb.velocity = new Vector2(0, 0);
	}

	internal void startPatrol()
    {
		//Sumador de tiempo
		currentTime += Time.deltaTime;

		//Si se vence el tiempo de patrulla gira hacia el otro lado
		if (currentTime > data.PatrolTime)
		{
			if (right)
			{

				right = false;
             
            }
			else
			{
				right = true;
               
            }

			//Y se resetea el tiempo
			currentTime = 0;
            
        }

		if (right)
		{
			//Muevo el enemigo a la derecha y giro el flip
			transform.Translate(Time.deltaTime * data.PatrolSpeed, 0, 0);
			GetComponent<SpriteRenderer>().flipX = data.XFlip;

			//Si los rayos a la derecha detectan al PJ, lo persigue
			ry = Physics2D.Raycast(castPointRight.position, Vector2.right, data.RayDistance);
			//Debug.DrawRay(castPointRight.position, Vector2.right * data.RayDistance, Color.green);

			if ((ry.collider != null) && (ry.collider.gameObject.CompareTag("Player")))
            {
				//Debug.DrawRay(castPointRight.position, Vector2.right * data.RayDistance, Color.red);
				ChasingPJ();
                currentTime = 0;

            }
        }
		else
		{
			transform.Translate(Time.deltaTime * -data.PatrolSpeed, 0, 0);
			GetComponent<SpriteRenderer>().flipX = !data.XFlip;

			ry = Physics2D.Raycast(castPointLeft.position, Vector2.left, data.RayDistance);
			//Debug.DrawRay(castPointRight.position, Vector2.right * data.RayDistance, Color.green);

			if ((ry.collider != null) && (ry.collider.gameObject.CompareTag("Player")))
			{
				//Debug.DrawRay(castPointRight.position, Vector2.right * data.RayDistance, Color.red);
                ChasingPJ();
                currentTime = 0;
            }

		}
	}

    internal void OnlyPatrol()
    {
        //Sumador de tiempo
        currentTime += Time.deltaTime;

        //Si se vence el tiempo de patrulla gira hacia el otro lado
        if (currentTime > data.PatrolTime)
        {
            if (right)
            {

                right = false;

            }
            else
            {
                right = true;

            }

            //Y se resetea el tiempo
            currentTime = 0;

        }

        if (right)
        {
            //Muevo el enemigo a la derecha y giro el flip
            transform.Translate(Time.deltaTime * data.PatrolSpeed, 0, 0);
            GetComponent<SpriteRenderer>().flipX = data.XFlip;

        }
        else
        {
            transform.Translate(Time.deltaTime * -data.PatrolSpeed, 0, 0);
            GetComponent<SpriteRenderer>().flipX = !data.XFlip;

            ry = Physics2D.Raycast(castPointLeft.position, Vector2.left, data.RayDistance);
            //Debug.DrawRay(castPointRight.position, Vector2.right * data.RayDistance, Color.green);

           

        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
	{
		//si la bola toca el enemigo que lo mate
		if (collision.gameObject.tag == "Ball")
		{

            OnSoundPlayed(data.DeathSound);

            OnDestroy();

        }

		//si las tramps tocan al enemigo los mata toca el enemigo que lo mate
		if (collision.gameObject.layer == 13)
		{
            OnSoundPlayed(data.DeathSound);

            OnDestroy();

        }


	}



    //Para matar al enemigo ponemos un trigger ya que la fireball esta en este modo
    private void OnTriggerEnter2D(Collider2D collision)
	{
		
		if (collision.gameObject.tag == "fireball")
		{
            OnSoundPlayed(data.DeathSound);


        }

        if(collision.gameObject.tag == "fireshield")

        {
            OnSoundPlayed(data.DeathSound);
          



        }


    }

    public void OnSoundPlayed(AudioClip audioClip)
    {
        SFX_Driver.Instance.PlaySound(audioClip);
    }
}
