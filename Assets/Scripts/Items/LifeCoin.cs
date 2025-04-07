using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCoin : MonoBehaviour, IAudioObserver
{
    [SerializeField] public int healValue = 10;
    [SerializeField] float patrolTime = 2.0f;
    [SerializeField] float patrolSpeed = 5.0f;
    bool up = true;
    //Llama al script de los stats
    public PlayerStats playerstats;
    float currentTime;

    [SerializeField] public int life_Score;
    public static event Action<int> OnScoreLifeChanged;


    [SerializeField] private AudioClip lifeSFX;



   

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
        // extraemos el componente para usar luego
        playerstats = FindObjectOfType<PlayerStats>();
        SFX_Driver.Instance.RegisterObserver(this);
    

}

    private void Update()
    {
        startPatrol();

    }




    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            OnSoundPlayed(lifeSFX);
            playerstats.GetHeal(healValue);

            OnScoreLifeChanged?.Invoke(life_Score);
            Destroy(gameObject);


        }


    }



    void startPatrol()
    {
        //Sumador de tiempo
        currentTime += Time.deltaTime;

        //Si se vence el tiempo de patrulla gira hacia el otro lado
        if (currentTime > patrolTime)
        {
            if (up) up = false;
            else up = true;

            //Y se resetea el tiempo
            currentTime = 0;

        }

        if (up)
        {
            //Muevo el life hacia arriba
            transform.Translate(0, Time.deltaTime * patrolSpeed, 0);

        }
        else
        {
            transform.Translate(0, Time.deltaTime * -patrolSpeed, 0);

        }
    }

}
