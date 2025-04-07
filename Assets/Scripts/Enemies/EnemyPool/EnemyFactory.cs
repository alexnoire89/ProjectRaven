using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : IEnemyFactory
{
    public Enemy CreateEnemy(GameObject enemyPrefab)
    {
        GameObject enemyObject = GameObject.Instantiate(enemyPrefab);
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        return enemy;
    }
}



