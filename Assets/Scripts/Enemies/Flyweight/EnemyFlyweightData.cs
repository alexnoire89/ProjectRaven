using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


[CreateAssetMenu(fileName = "New Enemy Flyweight", menuName = "Enemies/Enemy Flyweight Data")]

public class EnemyFlyweightData : ScriptableObject
{
    //Datos comunes de los enemigos
    [SerializeField] private int baseHealth;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private float patrolTime;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float rayDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool xFlip;
    [SerializeField] private int enemy_Score;
    [SerializeField] private GameObject lifecoin;
    [SerializeField] private Transform player;
    [SerializeField] private bool statePatrol;
    [SerializeField] private bool stateChase;
    [SerializeField] private bool stateIdle;


    public int BaseHealth => baseHealth;
    public AudioClip DeathSound => deathSound;
    public float PatrolTime => patrolTime;
    public float PatrolSpeed => patrolSpeed;
    public float RayDistance => rayDistance;
    public float MoveSpeed => Mathf.Abs(moveSpeed);
    public bool XFlip => xFlip;
    public int Enemy_Score => enemy_Score;
    public GameObject Lifecoin => lifecoin;
    public Transform Player => player;
    public bool StatePatrol => statePatrol;
    public bool StateChase => stateChase;
    public bool StateIdle => stateIdle;


}
