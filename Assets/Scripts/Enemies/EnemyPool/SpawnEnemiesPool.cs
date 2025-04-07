using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private float respawnTime = 2f;
    [SerializeField] private float respawnTimeLimit = 5f;
    [SerializeField] private int triggerDistanceCreation = 10;

    private List<Enemy> objectPool = new List<Enemy>();
    private IEnemyFactory enemyFactory;

    private float currentTime = 0f;
    private float currentTimeRespawn = 0f;
    private bool isActive = false;
    private float initPositionX;

    private void Start()
    {
        initPositionX = transform.position.x;
        enemyFactory = new EnemyFactory();
        InitializePool();

    }

    private void Update()
    {
        if (isActive)
        {
            currentTimeRespawn += Time.deltaTime;

            if (currentTimeRespawn < respawnTimeLimit)
            {
                CreateEnemies();
                currentTime += Time.deltaTime;
            }
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            //Fabrico un enemigo y lo agrego a la pool
            Enemy enemy = enemyFactory.CreateEnemy(enemyPrefab);
            objectPool.Add(enemy);
            enemy.gameObject.SetActive(false);
        }
    }

    private void CreateEnemies()
    {
       if (currentTime > respawnTime)
        {
          for (int i = 0; i < objectPool.Count; i++)
            {
               if (objectPool[i]!=null && !objectPool[i].gameObject.activeInHierarchy)
                {
                        
                   objectPool[i].transform.position = new Vector3(initPositionX + triggerDistanceCreation, transform.position.y - 1, 0);
                    objectPool[i].gameObject.SetActive(true);
                   currentTime = 0f;
                   break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isActive = true;
        }
    }
}
