using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCoin : MonoBehaviour, IAudioObserver
{

	
	[SerializeField] float patrolTime = 2.0f;
	[SerializeField] float patrolSpeed = 5.0f;
    [SerializeField] float lifeTime = 6.0f;


    bool up = true;
	//Llama al script de los stats
	public PlayerStats playerstats;
	float currentTime;

	[SerializeField] public int life_Score;
	public static event Action<int> OnScoreCoinChanged;


	[SerializeField] private AudioClip scoreSFX;




  

    void OnDestroy()
    {
        SFX_Driver.Instance.RemoveObserver(this);
    }

    public void OnSoundPlayed(AudioClip audioClip)
    {
        SFX_Driver.Instance.PlaySoundWithStop(audioClip);
    }

    void Start()
	{
		// extraemos el componente para usar luego
		playerstats = FindObjectOfType<PlayerStats>();
        SFX_Driver.Instance.RegisterObserver(this);
    

}

	private void Update()
	{
		startPatrol();
        Destroy(gameObject, lifeTime);

    }




    private void OnTriggerEnter2D(Collider2D collision)
	{

		if (collision.gameObject.tag == "Player")
		{
            OnSoundPlayed(scoreSFX);
			//playerstats.GetHeal(healValue);

			OnScoreCoinChanged?.Invoke(life_Score);
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