using UnityEngine;

public class IdleState : IEnemyState
{
    float _idleTime;
    float _timer;

    public void EnterState(EnemyStateManager enemy)
    {
        enemy.GetComponent<Animator>().Play("Idle");
        _idleTime = Random.Range(1f, 4f);
        _timer = 0f;
    }

    public void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("[Idle State] : State Exited");
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        _timer += Time.deltaTime;
        if(_timer >= _idleTime)
        {
            enemy.TransitionToState(new PatrolState());
            return;
        }
    }

}
