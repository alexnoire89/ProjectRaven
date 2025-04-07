using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChaseState : IEnemyState
{
    private EnemyStates enemy;

    public ChaseState(EnemyStates enemy)
    {
        this.enemy = enemy;
    }

    public void UpdateState()
    {
        enemy.ChasingPJ();
    }
}
