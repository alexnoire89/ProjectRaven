
using UnityEngine;

public class PatrolState : IEnemyState
{
    private EnemyStates enemy;

    public PatrolState(EnemyStates enemy)
    {
        this.enemy = enemy;
    }

    public void UpdateState()
    {
        enemy.Patrol();
    }
}