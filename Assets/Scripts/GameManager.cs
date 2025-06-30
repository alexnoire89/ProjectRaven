using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IAudioObserver { 



    public static GameManager Instance;
    private Animator animator;
    [SerializeField] private AnimationClip TransitionClip;

    //[SerializeField] public Score score;
    //[SerializeField] public LifeBarUI lifebar;
    //[SerializeField] public Lifes lifesUI;
    [SerializeField] public PlayerUIFacade playerUIFacade;

    [SerializeField] Transform playerObj;
    [SerializeField] GameObject player;

    [SerializeField] private AudioClip BGM_Menu;
    [SerializeField] private AudioClip BGM_Game;
    [SerializeField] private AudioClip BGM_GameOver;
    [SerializeField] private AudioClip BGM_Victory;

    [SerializeField] public int lifes = 3;
    [SerializeField] public int HP = 100;
    private int currentHP;

    [SerializeField] public int positionXRespawn = 214;
    [SerializeField] public int positionYRespawn = 262;




    bool startPressed = false;

    float currentTime;


  

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
        animator = GetComponent<Animator>();
        player = GetComponent<GameObject>();
        SFX_Driver.Instance.RegisterObserver(this);
        currentHP = HP;

        //Resetear Tuto
        PlayerPrefs.SetInt("TutorialDone", 0);
        PlayerPrefs.Save();
    }


    private void Update()
    {
        if (startPressed)
        {
            currentTime += Time.deltaTime;



            //if (currentTime > 4)
            //{

            //    //SFX_Driver.Instance.StopSound();
            //    SFX_Driver.Instance.CrossfadeMusic(BGM_Game, 2f);

            //    //Se resetea score para proxima partida
            //    SaveDataSystem.Instance.ResetSessionScore();


            //    SceneManager.LoadScene("Game");

            //    startPressed = false;

            //}

            if (currentTime > 4)
            {
                SFX_Driver.Instance.CrossfadeMusic(BGM_Game, 2f);

                bool tutorialCompleted = PlayerPrefs.GetInt("TutorialDone", 0) == 1;

                if (tutorialCompleted)
                {
                    SaveDataSystem.Instance.ResetSessionScore();
                    SceneManager.LoadScene("Game");
                }
                else
                {
                    SceneManager.LoadScene("TrainingRoom");
                }

                startPressed = false;
            }

        }


    }


    private void OnEnable()
    {
        PlayerStats.OnDeath += GameOver;
        PlayerStats.OnLifeChanged += ManageHP;
        Boss_Skelleton.OnVictory += VictoryFinal;

        Enemy.OnScoreChanged += ManageScore;
        LifeCoin.OnScoreLifeChanged += ManageScore;
        ScoreCoin.OnScoreCoinChanged += ManageScore;
        Boss_Skelleton.OnScoreBossChanged += ManageScore;

    }

    private void OnDisable()
    {
        PlayerStats.OnDeath -= GameOver;
        PlayerStats.OnLifeChanged -= ManageHP;
        Boss_Skelleton.OnVictory -= VictoryFinal;

        Enemy.OnScoreChanged -= ManageScore;
        LifeCoin.OnScoreLifeChanged -= ManageScore;
        ScoreCoin.OnScoreCoinChanged -= ManageScore;
        Boss_Skelleton.OnScoreBossChanged -= ManageScore;
    }


    public void ManageHP(int damage)
    {
        //lifebar.ChangeActualHP(damage);
        playerUIFacade.ChangeActualHP(damage);
    }


    public void ManageScore(int points)
    {
        playerUIFacade.AddPoints(points);
       
    }


    public void PlayGame()
    {
        //level 1
        //Debug.Log("level 1");
        startPressed = true;
        
    }

    public void ToMenu()
    {
        //Debug.Log("Menu");

        animator.SetTrigger("isStart");

        //SFX_Driver.Instance.StopSound();
        //OnSoundPlayed(BGM_Menu);

        SFX_Driver.Instance.CrossfadeMusic(BGM_Menu, 1f);


        SceneManager.LoadScene("Menu");
        
    }


    public void Exit()
    {
        //Debug.Log("Ha Salido");

        animator.SetTrigger("isStart");

        SFX_Driver.Instance.StopSound();

        Application.Quit();

    }


    private void GameOver()
    {

        SavingData();


        if (lifes > 0)
        {
            lifes--;
            playerUIFacade.RemoveLifes(1);
            playerUIFacade.ResetPoints();
            playerUIFacade.InitLifeBar(HP);

            playerObj.transform.position = new Vector3(positionXRespawn, positionYRespawn, 0);
            animator.SetTrigger("isStart");

            //Reseteamos el porton
            FinalGate finalGate = FindObjectOfType<FinalGate>();
            if (finalGate != null)
            {
                finalGate.ResetDoor();
            }
        }

        else 
        {

            playerUIFacade.ResetPoints();
            Destroy(player, 1f);
            

            animator.SetTrigger("isStart");

            //SFX_Driver.Instance.StopSound();
            //OnSoundPlayed(BGM_GameOver);
            SFX_Driver.Instance.CrossfadeMusic(BGM_GameOver, 1f);
            



            SceneManager.LoadScene("GameOver");

        }

    }


    public void VictoryFinal()
    {
        SavingData();

        animator.SetTrigger("isStart");
        //SFX_Driver.Instance.StopSound();
        //OnSoundPlayed(BGM_Victory);

        SFX_Driver.Instance.CrossfadeMusic(BGM_Victory, 1f);

        SceneManager.LoadScene("Victory");
    }

    private void SavingData()
    {

        //Se salva puntaje
        int finalScore = playerUIFacade.GetCurrentScore();
        SaveDataSystem.Instance.TrySaveHighScore(finalScore);

        SaveDataSystem.Instance.UpdateSessionBestScore(playerUIFacade.GetCurrentScore());


    }


}
