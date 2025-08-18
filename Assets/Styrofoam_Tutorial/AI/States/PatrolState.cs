using UnityEngine;

public class PatrolState : IEnemyState
{
    float _moveTime = 0f;
    float _moveTimer = 0f;
    bool _isPatrolling = false;

    public void EnterState(EnemyStateManager enemy)
    {
        enemy.GetComponent<Animator>().Play("Patrol");
        _isPatrolling = false;
        _moveTimer = 0f;
        _moveTime = Random.Range(1f, 4f);
    }

    public void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("[Patrol State] : State Exited");
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.GetComponent<StatBehaviour>().CurrentHP <= 0)
        {
            return;
        }

        if (enemy.GetComponent<EnemySight>().IsPlayerInRange())
        {
            enemy.TransitionToState(new ChaseState());
            return;
        }

        if (!_isPatrolling)
        {
            enemy.GetComponent<EnemyFlip>().Flip();
            _isPatrolling = true;
        }
        Vector2 dir = enemy.transform.GetChild(0).localScale.x < 0 ? Vector2.right : Vector2.left;
        _moveTimer += Time.deltaTime;
        enemy._rb.linearVelocity = new Vector2(dir.x * enemy.GetComponent<EnemyDataManager>()._enemyData.PatrolSpeed, 0f);

        if(_moveTimer >= _moveTime)
        {
            enemy._rb.linearVelocity = Vector2.zero;
            enemy.TransitionToState(new IdleState());
        }
    }
}
