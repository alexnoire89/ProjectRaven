using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyFactory
{
    Enemy CreateEnemy(GameObject enemyPrefab);
}

