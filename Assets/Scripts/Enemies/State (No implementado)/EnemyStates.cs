
using UnityEngine;

public class EnemyStates : MonoBehaviour
{
    private IEnemyState currentState;
    public PatrolState patrolState;
    public ChaseState chaseState;

    void Start()
    {
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        currentState = patrolState; // El estado inicial es patrulla
    }

    void Update()
    {
        currentState.UpdateState();
    }

    public void SetState(IEnemyState state)
    {
        currentState = state;
    }

    public void ChasingPJ() { 
    
    
    
    }

    public void Patrol()
    {



    }
}
